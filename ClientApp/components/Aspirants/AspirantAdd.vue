<template id="aspirant">
  <div>
    <h2>Add new aspirant</h2>

    <b-container fluid>
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Прізвище:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm" 
          type="text" 
          name="surename"
          v-model="surename"
          v-validate="'required|alpha'"
          :class="{'has-error': errors.has('surename')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('surename')" class="offset-3 alert-validate" >{{ errors.first('surename') }}</div>
         
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Ім'я:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"            
            name="name"
            v-model="name"
            v-validate="'required|alpha'"
            :class="{'has-error': errors.has('name')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('name')" class="offset-3 alert-validate" >{{ errors.first('name') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">По батькові:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"           
            name="patronymic"
          v-model="patronymic"
          v-validate="'alpha'"
          :class="{'has-error': errors.has('patronymic')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('patronymic')" class="offset-3 alert-validate" >{{ errors.first('patronymic') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Телефон:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"                        
            name="phone"
          v-model="phone"
          v-validate="'alpha'"
          :class="{'has-error': errors.has('phone')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('phone')" class="offset-3 alert-validate" >{{ errors.first('phone') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Email:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="email"
            name="email"
          v-model="email"
          v-validate="'email'"
          :class="{'has-error': errors.has('email')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('email')" class="offset-3 alert-validate" >{{ errors.first('email') }}</div>


      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Дата народження:</b-col>
        <b-col cols="9">
          <b-form-input
            name="birthday"
            type="date"
            v-model="birthday"
            size="sm"            
            min="1935-01-01"
            max="2000-12-31"
            v-validate="'required|date_format:YYYY-MM-DD|date_between:1935-01-01,2000-12-31'"
            value="1998-01-01"
          :class="{'has-error': errors.has('birthday')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('birthday')" class="offset-3 alert-validate" >{{ errors.first('birthday') }}</div>


      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Докторант:</b-col>
        <b-col cols="9">
          <span>
            <input type="checkbox" class="short-check" id="doctorant" v-model="doctorant">
          </span>
        </b-col>
      </b-row>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Спеціальність:</b-col>
        <b-col cols="9">
          <v-select label="text" :options="specialities" v-model="selectedSpeciality">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
    </b-container>

    <button class="btn btn-warning" v-on:click="createAspirant('create')">Create</button>
    <button class="btn btn-warning" v-on:click="createAspirant('cancel')">Cancel</button>
    <!-- <router-link class="btn btn-warning" to="/aspirants">Cancel</router-link> -->
    <hr>
  </div>
</template>

<script>
import axios from "axios";
import { eventBus } from "./../../index.js";
import vSelect from "vue-select";

export default {
  components: {
    vSelect
  },
  data() {
    return {
      //   errors: [],
      name: "",
      surename: "",
      patronymic: "",
      phone: "",
      email: "",
      birthday: "",
      inputdate: "",
      specialityid: "",
      doctorant: false,
      selectedSpeciality: "",
      statuses: [],
      specialities: []
    };
  },

  created: function() {
    var self = this;
    axios.get(`api/specialities/names`).then(function(response) {
      self.specialities = response.data;
    });
    // .catch(function(e) {
    //   self.errors.push(e);
    // });
  },

  methods: {
    createAspirant: function(action) {
      var backparam;

      this.$validator.validate().then(valid => {
        if (valid || action == "cancel") {
          if (action == "create") {
            backparam = {
              name: this.name,
              surename: this.surename,
              patronymic: this.patronymic,
              phone: this.phone,
              email: this.email,
              birthday: this.birthday,
              specialityid: this.selectedSpeciality.value,
              doctorant: this.doctorant,
              action: "create"
            };
          } else {
            backparam = {
              action: "cancel"
            };
          }
          eventBus.$emit("add-aspirant", backparam);
          this.$router.push("/aspirants");
        }
      });
    }
  }
};
</script>