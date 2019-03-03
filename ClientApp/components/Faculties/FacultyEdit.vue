<template>
  <div class="col-10 offset-1">

<form v-on:submit.prevent="editItem">    
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

<b-row class="pad-4">
  <input type="submit" class="btn btn-warning mr-2" value="Зберегти"/>
  <router-link to="/faculties" tag="button" class="btn btn-warning" >Скасувати</router-link>  
</b-row>

</b-container>
</form>
</div>   
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      model: {}
    };
  },

  mounted: function() {
    this.model = Object.assign({}, this.$route.params.item);
  },

  methods: {
    editItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          axios.put("api/faculties/" + this.model.id, this.model);
          this.$router.push("/faculties");
        }
      });
    }
  }
};
</script>