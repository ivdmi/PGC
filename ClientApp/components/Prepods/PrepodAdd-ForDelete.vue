<template>
  <div class="col-10 offset-1">

<form v-on:submit.prevent="addItem">    
      <b-container fluid>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Назва:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm"
          name="name"
          v-model="model.name"
          v-validate="'required'"
          :class="{'has-error': errors.has('name')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('name')" class="offset-3 alert-validate" >{{ errors.first('name') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Скорочення:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm"
          name="acronym"
          v-model="model.acronym"
          v-validate="'required'"
          :class="{'has-error': errors.has('acronym')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('acronym')" class="offset-3 alert-validate" >{{ errors.first('acronym') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Факультет:</b-col>
        <b-col cols="9">
          <v-select 
          name="Факультет" 
          label="text" 
          :options="faculties" 
          v-model="selectedFaculty"
          v-validate="'required|selectValue'"
          :class="{'has-error': errors.has('Факультет')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Факультет')" class="offset-3 alert-validate" >{{ errors.first('Факультет') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Декан:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm"
          name="decan"
          v-model="model.decan"         
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('decan')" class="offset-3 alert-validate" >{{ errors.first('decan') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Телефон:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"                        
            name="Телефон"
          v-model="model.phone"
          v-validate="'phone'"
          :class="{'has-error': errors.has('Телефон')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Телефон')" class="offset-3 alert-validate" >{{ errors.first('Телефон') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Телефон2:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"                        
            name="Телефон2"
          v-model="model.phone2"
          v-validate="'phone'"
          :class="{'has-error': errors.has('Телефон2')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Телефон2')" class="offset-3 alert-validate" >{{ errors.first('Телефон2') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Email:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="email"
            name="Email"
          v-model="model.email"
          v-validate="'email'"
          :class="{'has-error': errors.has('Email')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Email')" class="offset-3 alert-validate" >{{ errors.first('Email') }}</div>     

<div class="col-10 offset-2">
<h5>Відкриті спеціальності</h5>
<department-specialities-selected-list @del-speciality="onDel" :list="this.model.specialities"></department-specialities-selected-list>
<br>
<b-row class="pad-4">
  <input type="submit" class="btn btn-warning mr-2" value="Додати кафедру"/>
  <router-link to="/departments" tag="button" class="btn btn-warning" >Скасувати</router-link>  
</b-row>
<hr>
<h5>Додати нові спеціальності</h5>
<department-specialities-list @add-specialities="onAdd" @add-speciality="onAddOne" ></department-specialities-list>
</div>
</b-container>
</form>
</div>   
</template>

<script>
import axios from "axios";
import vSelect from "vue-select";
import prepodDepartmentList from "./PrepodDepartmentList";
import prepodDepartmentSelectedList from "./PrepodDepartmentSelectedList";

export default {
  components: {
    vSelect,
    prepodDepartmentList,
    prepodDepartmentSelectedList
  },

  data() {
    return {
      model: {
        name: "",
        acronym: "",
        decan: "",
        phone: "",
        phone2: "",
        email: "",
        facultyId: "",
        specialities: []
      },
      selectedFaculty: "",
      faculties: []
    };
  },

  mounted: function() {
    axios.get(`api/faculties/names`).then(response => {
      this.faculties = response.data;
    });
  },

  methods: {
    addItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.model.facultyId = this.selectedFaculty.value;
          axios.post("api/departments", this.model).then(response => {
            this.$router.push("/departments");
          });
        }
      });
    },

    // Возвращаем данные (список специальностей) из подчиненной формы
    // метод onAdd вызывается по событию add-department
    onAdd(data) {
      data.forEach(item => {
        var k = this.model.specialities
          .map(function(e) {
            return e.id;
          })
          .indexOf(item.id);
        if (k === -1) {
          this.model.specialities.push(item);
        }
      });
    },

    onDel(id) {
      var k = this.model.specialities
        .map(function(e) {
          return e.id;
        })
        .indexOf(id);
      if (k != -1) {
        this.model.specialities.splice(k, 1);
      }
    },

    onAddOne(data) {
      var k = this.model.specialities
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (k === -1) {
        this.model.specialities.push(data);
      }
    }
  }
};
</script>