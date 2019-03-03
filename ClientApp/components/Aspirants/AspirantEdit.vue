<template id="aspirant">
  <div>
    <h2>Редагування облікового запису аспіранта</h2>

    <b-container fluid>
      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Прізвище:</b-col>
        <b-col cols="5">
          <b-form-input 
          size="sm" 
          type="text" 
          name="surename"
          v-model="aspirant.surename"
          v-validate="'required|alpha'"
          :class="{'has-error': errors.has('surename')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('surename')" class="offset-3 alert-validate" >{{ errors.first('surename') }}</div>
         
      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Ім'я:</b-col>
        <b-col cols="5">
          <b-form-input
            size="sm"
            type="text"            
            name="name"
            v-model="aspirant.name"
            v-validate="'required|alpha'"
            :class="{'has-error': errors.has('name')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('name')" class="offset-3 alert-validate" >{{ errors.first('name') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">По батькові:</b-col>
        <b-col cols="5">
          <b-form-input
            size="sm"
            type="text"           
            name="patronymic"
          v-model="aspirant.patronymic"
          v-validate="'alpha'"
          :class="{'has-error': errors.has('patronymic')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('patronymic')" class="offset-3 alert-validate" >{{ errors.first('patronymic') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Телефон:</b-col>
        <b-col cols="5">
          <b-form-input
            size="sm"
            type="text"                        
            name="phone"
          v-model="aspirant.phone"
          v-validate="'phone'"
          :class="{'has-error': errors.has('phone')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('phone')" class="offset-3 alert-validate" >{{ errors.first('phone') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Email:</b-col>
        <b-col cols="5">
          <b-form-input
            size="sm"
            type="email"
            name="email"
          v-model="aspirant.email"
          v-validate="'email'"
          :class="{'has-error': errors.has('email')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('email')" class="offset-3 alert-validate" >{{ errors.first('email') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Народження:</b-col>
        <b-col cols="5">
          <b-form-input
            name="birthday"
            type="date"
            v-model="aspirant.birthday"
            size="sm"            
            :min=this.minDate
            :max=this.maxDate
            v-validate="{
              date_format : 'YYYY-MM-DD',
              date_between : [this.minDate, this.maxDate, true]
            }"
          :class="{'has-error': errors.has('birthday')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('birthday')" class="offset-3 alert-validate" >{{ errors.first('birthday') }}</div>

<b-row class="pad-4">
        <b-col cols="2" class="text-right">inputDate:</b-col>
        <b-col cols="5">
          <b-form-input
            name="inputDate"
            type="date"
            v-model="aspirant.inputDate"
            size="sm"            
            min="2017-01-01"
            :max=this.currentDate
            v-validate="{
              date_format : 'YYYY-MM-DD',
              date_between : ['2017-01-01', this.currentDate, true]
            }"
          :class="{'has-error': errors.has('inputDate')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('inputDate')" class="offset-3 alert-validate" >{{ errors.first('inputDate') }}</div>

<b-row class="pad-4">
        <b-col cols="2" class="text-right">Захист:</b-col>
        <b-col cols="5">
          <b-form-input
            name="protectionDate"
            type="date"
            v-model="aspirant.protectionDate"
            size="sm"            
            min="2017-01-01"
            :max=this.currentDate
            v-validate="{
              date_format : 'YYYY-MM-DD',
              date_between : ['2017-01-01', this.currentDate, true]
            }"
          :class="{'has-error': errors.has('protectionDate')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('protectionDate')" class="offset-3 alert-validate" >{{ errors.first('protectionDate') }}</div>
   
      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Спеціальність:</b-col>
        <b-col cols="5">
          <v-select name="Спеціальність" label="text" :options="specialities" v-model="selectedSpeciality" v-validate="'required'" :class="{'has-error': errors.has('Спеціальність')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Спеціальність')" class="offset-3 alert-validate" >{{ errors.first('Спеціальність') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Форма:</b-col>
        <b-col cols="5">
          <v-select 
          name="Форма" 
          label="text" 
          :options="statuses" 
          v-model="selectedStatus"
          v-validate="'required'" 
          :class="{'has-error': errors.has('Форма')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Форма')" class="offset-3 alert-validate" >{{ errors.first('Форма') }}</div>

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Статус:</b-col>
        <b-col cols="5">
          <v-select 
          name="Статус" 
          label="text" 
          :options="statuses" 
          v-model="selectedStatus"
          v-validate="'required'" 
          :class="{'has-error': errors.has('Статус')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Статус')" class="offset-3 alert-validate" >{{ errors.first('Статус') }}</div>      
      <b-row class="pad-4">
        <b-col cols="2" class="text-right">В наявності:</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.present">
          </span>
        </b-col>
      </b-row>         

<b-row class="pad-4">
        <b-col cols="2" class="text-right">Бюджет (контракт):</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.budget">
          </span>
        </b-col>
      </b-row>         

<b-row class="pad-4">
        <b-col cols="2" class="text-right">Стаціонар (заочна):</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.stationary">
          </span>
        </b-col>
      </b-row>         

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Денна (вечірня):</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.day">
          </span>
        </b-col>
      </b-row>         

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Докторант (аспірант):</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.doctorant">
          </span>
        </b-col>
      </b-row>  

      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Захист:</b-col>
        <b-col cols="5">
          <span>
            <input type="checkbox" class="short-check" v-model="aspirant.protection">
          </span>
        </b-col>
      </b-row>       
                
      <b-row class="pad-4">
        <b-col cols="2" class="text-right">Курс:</b-col>
        <b-col cols="5">
          <b-form-input          
          name="Курс"
          v-model="aspirant.course"
          min=0
          max=3
          type="number"
          v-validate="'numeric|between:0,3'"
          :class="{'has-error': errors.has('Курс')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Курс')" class="offset-3 alert-validate" >{{ errors.first('Курс') }}</div>

    </b-container>

    <button class="btn btn-warning" v-on:click="saveAspirant('put')">Зберегти</button>
    <button class="btn btn-warning" v-on:click="saveAspirant('cancel')">Відмовитися</button>

    <hr>
  </div>
</template>

<script>
import axios from "axios";
import { eventBus } from "./../../index.js";
import vSelect from "vue-select";
import VueRouter from "vue-router";
import moment from "moment";

export default {
  components: {
    vSelect
  },

  data() {
    return {
      // новый объект для значений (чтобы не портить ссылку)
      aspirant: {
        id: this.$route.params.item.id,

        name: this.$route.params.item.name,
        surename: this.$route.params.item.surename,
        patronymic: this.$route.params.item.patronymic,

        phone: this.$route.params.item.phone,
        email: this.$route.params.item.email,

        birthday: this.$route.params.item.birthday,
        inputDate: this.$route.params.item.inputDate,
        protectionDate: this.$route.params.item.protectionDate,

        present: this.$route.params.item.present,
        budget: this.$route.params.item.budget,
        stationary: this.$route.params.item.stationary,
        day: this.$route.params.item.day,
        doctorant: this.$route.params.item.doctorant,
        protection: this.$route.params.item.protection,        

        course: this.$route.params.item.course,

        specialityId: this.$route.params.item.specialityId,
        statusType: this.$route.params.item.statusType,
        studyForm: this.$route.params.item.studyForm,
      },

      // запомнить ссылку на объект
      refAspirant: this.$route.params.item,

      specialities: [],
      statuses: [],
      studyforms: [],
      selectedSpeciality: {
        value: this.$route.params.item.specialityId
      },
      selectedStatus: {
        value: this.$route.params.item.statusType
      },
      currentDate: moment().format("YYYY-MM-DD"),
      maxDate: moment()
        .add("years", -20)
        .format("YYYY-MM-DD"),
      minDate: "1935-01-01"
    };
  },

  mounted: function() {
    // читаем в массив специальности и заполняем selectedSpeciality

    this.specialities = this.$route.params.specialities;
    this.selectedSpeciality.text = this.specialities.find(
      x => x.value === this.selectedSpeciality.value
    ).text;

    this.statuses = this.$route.params.statuses;
    this.studyforms = this.$route.params.studyforms;
    this.selectedStatus.text = this.statuses.find(
      x => x.value === this.$route.params.item.statusType
    ).text;
    this.selectedStudyform.text = this.studyforms.find(
      x => x.value === this.$route.params.item.studyForm
    ).text;
    // var self = this;

    // axios.get(`api/specialities/names`).then(function(response) {
    //   self.specialities = response.data;
    //   self.selectedSpeciality.text = self.specialities.find(
    //     x => x.value === self.selectedSpeciality.value
    //   ).text;
    // });

    // axios.get(`api/Aspirants/statuses`).then(function(response) {
    //   self.statuses = response.data;
    //   self.selectedStatus.text = self.statuses.find(
    //     x => x.value === self.$route.params.item.statustypeId
    //   ).text;
    // });
  },

  methods: {
    saveAspirant: function(action) {
      var backparam;

      this.$validator.validate().then(valid => {
        if (valid || action == "cancel") {
          if (action == "put") {
            this.aspirant.specialityId = this.selectedSpeciality.value;
            this.aspirant.statusType = this.selectedStatus.value;

            // заполнить ссылку валидными значениями
            this.refAspirant.name = this.aspirant.name;
            this.refAspirant.surename = this.aspirant.surename;
            this.refAspirant.patronymic = this.aspirant.patronymic;
            this.refAspirant.phone = this.aspirant.phone;
            this.refAspirant.email = this.aspirant.email;

            this.refAspirant.birthday = this.aspirant.birthday;
            this.refAspirant.inputDate = this.aspirant.inputDate;
            this.refAspirant.protectionDate = this.aspirant.protectionDate;

            this.refAspirant.specialityId = this.selectedSpeciality.value;
            this.refAspirant.statusType = this.selectedStatus.value;

            this.refAspirant.present = this.aspirant.present;
            this.refAspirant.budget = this.aspirant.budget;
            this.refAspirant.stationary = this.aspirant.stationary;
            this.refAspirant.day = this.aspirant.day;
            
            this.refAspirant.doctorant = this.aspirant.doctorant;
            this.refAspirant.protection = this.aspirant.protection;
            
            this.refAspirant.course = this.aspirant.course;

            backparam = {
              item: this.aspirant,
              action: "put"
            };
          } else {
            backparam = {
              action: "cancel"
            };
          }
          
          console.log(backparam);

          eventBus.$emit("change-aspirant", backparam);
          this.$router.push("/aspirants");
        }
      });
    }
  }
};
</script>