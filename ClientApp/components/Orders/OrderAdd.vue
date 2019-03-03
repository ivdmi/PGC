<template>
  <div class="col-10 offset-1">
    <h2>Додати наказ</h2>

<form v-on:submit.prevent="addItem">    
       <b-container fluid>
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Номер:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm"
          name="number"
          v-model="order.number"
          v-validate="'required|numeric'"
          :class="{'has-error': errors.has('number')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('number')" class="offset-3 alert-validate" >{{ errors.first('number') }}</div>
      
<b-row class="pad-4">
        <b-col cols="3" class="text-right">Дата наказу:</b-col>
        <b-col cols="9">
          <b-form-input
            name="Дата"
            type="date"
            v-model="order.date"
            size="sm"            
            min="2019-01-01"
            :max=this.currentDate
            v-validate="{
              required,
              date_format : 'YYYY-MM-DD',
              date_between : ['2019-01-01', this.currentDate, true]
            }"         
          :class="{'has-error': errors.has('Дата')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('Дата')" class="offset-3 alert-validate" >{{ errors.first('Дата') }}</div>

<b-row class="pad-4">
        <b-col cols="3" class="text-right">Зміст:</b-col>
        <b-col cols="9">
          <b-form-input v-model="order.context" size="sm" ></b-form-input>
        </b-col>
      </b-row>   
  
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Вид наказу:</b-col>
        <b-col cols="9">
          <v-select 
          name="Вид" 
          label="text" 
          :options="types"
          v-model="selectedType"
          v-validate="'required'" 
          :class="{'has-error': errors.has('Вид')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Вид')" class="offset-3 alert-validate" >{{ errors.first('Вид') }}</div>
  <br />

  <div class="form-group">
    <input type="submit" class="btn btn-primary" value="Додати наказ"/>
  </div>

<aspirant-order-list @add-aspirants="onAdd" ></aspirant-order-list>

  </b-container>
  </form>
  </div>
   
</template>

<script>
import axios from "axios";
import moment from "moment";
import vSelect from "vue-select";
import aspirantOrderList from "./AspirantOrderList";

export default {
  components: {    
    name: "AddItem",
    vSelect,
    aspirantOrderList
  },

  data() {
    return {
      order: {
        number: "",
        context: "",
        ordertypeId: "",
        date: "",
        aspirants: []
      },
      selectedType: "",      
      types: [],
      currentDate: moment().format("YYYY-MM-DD")
    };
  },

  created: function() {
    this.types = this.$route.params.types;
    },

  methods: {
    addItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.order.ordertypeId = this.selectedType.value;
          axios.post("api/orders", this.order).then(response => {
            console.log(response);
            this.$router.push("/orders");
      });
    }
  })
  },

// Возвращаем данные (список аспирантов) из подчиненной формы
// метод onAdd вызывается по событию add-order
    onAdd(data){
      this.order.aspirants = data;     
    }
  }
};
</script>