<template>
  <div class="col-10 offset-1">
    <form v-on:submit.prevent="addItem">
      <b-container fluid>
        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Прізвище:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="surename"
              v-model="model.surename"
              v-validate="'required|alpha'"
              :class="{'has-error': errors.has('surename')}"
            ></b-form-input>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('surename')"
          class="offset-3 alert-validate"
        >{{ errors.first('surename') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Ім'я:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="name"
              v-model="model.name"
              v-validate="'required|alpha'"
              :class="{'has-error': errors.has('name')}"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('name')" class="offset-3 alert-validate">{{ errors.first('name') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">По батькові:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="patronymic"
              v-model="model.patronymic"
              v-validate="'alpha'"
              :class="{'has-error': errors.has('patronymic')}"
            ></b-form-input>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('patronymic')"
          class="offset-3 alert-validate"
        >{{ errors.first('patronymic') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Ступінь:</b-col>
          <b-col cols="5">
            <v-select
              name="Ступінь"
              label="text"
              :options="degrees"
              v-model="selectedDegree"
              v-validate="'required|selectValue'"
              :class="{'has-error': errors.has('Ступінь')}"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('Ступінь')"
          class="offset-3 alert-validate"
        >{{ errors.first('Ступінь') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Звання:</b-col>
          <b-col cols="5">
            <v-select
              name="Звання"
              label="text"
              :options="ranks"
              v-model="selectedRank"
              v-validate="'required|selectValue'"
              :class="{'has-error': errors.has('Звання')}"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('Звання')"
          class="offset-3 alert-validate"
        >{{ errors.first('Звання') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Посада:</b-col>
          <b-col cols="5">
            <v-select
              name="Посада"
              label="text"
              :options="positions"
              v-model="selectedPosition"
              v-validate="'required|selectValue'"
              :class="{'has-error': errors.has('Посада')}"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('Посада')"
          class="offset-3 alert-validate"
        >{{ errors.first('Посада') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Народження:</b-col>
          <b-col cols="5">
            <b-form-input
              name="Народження"
              type="date"
              v-model="model.birthday"
              size="sm"
              :min="this.minDate"
              :max="this.maxDate"
              v-validate="{
              date_format : 'YYYY-MM-DD',
              date_between : [this.minDate, this.maxDate, true]
            }"
              :class="{'has-error': errors.has('Народження')}"
            ></b-form-input>
          </b-col>
        </b-row>
        <div
          v-if="errors.has('Народження')"
          class="offset-3 alert-validate"
        >{{ errors.first('Народження') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Телефон:</b-col>
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
        <div
          v-if="errors.has('Телефон')"
          class="offset-3 alert-validate"
        >{{ errors.first('Телефон') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Email:</b-col>
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
        <div v-if="errors.has('Email')" class="offset-3 alert-validate">{{ errors.first('Email') }}</div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">В наявності:</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.present">
            </span>
          </b-col>
        </b-row>

        <div class="col-10 offset-2">
          <h5>Місця роботи</h5>
          <prepod-department-selected-list @del-department="onDel" :list="this.model.departments"></prepod-department-selected-list>

          <br>
          <b-row class="pad-4">
            <input type="submit" class="btn btn-warning mr-2" value="Зберегти">
            <router-link to="/prepods" tag="button" class="btn btn-warning">Скасувати</router-link>
          </b-row>
		  
		  
          <hr>
          <h5>Обрати кафедри</h5>
          <prepod-department-list @add-department="onAddOne"></prepod-department-list>
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
import moment from "moment";

export default {
  components: {
    vSelect,
    prepodDepartmentList,
    prepodDepartmentSelectedList
  },

  data() {
    return {
      model: {
        surename: "",
        name: "",
        patronymic: "",

        rank: "",
        degree: "",
        position: "",

        birthday: "",
        phone: "",
        email: "",
        present: true,
        departments: []
      },

      selectedRank: {
        value: 0,
        text: "-"
      },
      selectedDegree: {
        value: 0,
        text: "-"
      },
      selectedPosition: {
        value: 6,
        text: "професор"
      },
      ranks: [],
      degrees: [],
      positions: [],

      maxDate: moment()
        .add("years", -25)
        .format("YYYY-MM-DD"),
      minDate: "1930-01-01"
    };
  },

  mounted: function() {
    axios.get("api/Prepods/lists/0").then(response => {
      this.ranks = response.data.ranks;
      this.degrees = response.data.degrees;
      this.positions = response.data.positions;
    });
  },

  methods: {
    addItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.model.rank = this.selectedRank.value;
          this.model.degree = this.selectedDegree.value;
          this.model.position = this.selectedPosition.value;
          axios.post("api/Prepods/", this.model).then(response => {
            this.$router.push("/prepods");
          });
        }
      });
    },

    onDel(id) {
      var k = this.model.departments
        .map(function(e) {
          return e.id;
        })
        .indexOf(id);
      if (k != -1) {
        this.model.departments.splice(k, 1);
      }
    },

    onAddOne(data) {
      var k = this.model.departments
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (k === -1) {
        this.model.departments.push(data);
      }
    }
  }
};
</script>