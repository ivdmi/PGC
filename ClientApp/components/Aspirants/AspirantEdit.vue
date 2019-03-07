<template>
  <div class="col-10 offset-1">
    <h2>Редагування облікового запису аспіранта</h2>
    <form v-on:submit.prevent="editItem">
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
          <b-col cols="2" class="text-right">Бюджет (контракт)</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.budget">
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Докторант (аспірант)</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.doctorant">
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Чоловік (жінка)</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.sex">
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Стать - чоловіча</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.sex">
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">В наявності:</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.present">
            </span>
          </b-col>
        </b-row>

        <div class="col-10 offset-2">
          <br>
          <b-row class="pad-4">
            <input type="submit" class="btn btn-warning mr-2" value="Зберегти">
            <router-link to="/prepods" tag="button" class="btn btn-warning">Скасувати</router-link>
          </b-row>
          <hr>
        </div>
      </b-container>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import vSelect from "vue-select";
import moment from "moment";

export default {
  components: {
    vSelect
  },

  data() {
    return {
      model: {
        id: "",
        surename: "",
        name: "",
        patronymic: "",

        birthday: "",

        phone: "",
        email: "",

        // date
        inputDate: "",
        graduationDate: "",
        protectionDate: "",

        // bool
        stationary: "",
        budget: "",
        doctorant: "",
        sex: "",
        protection: "",
        present: "",

        // int (numeric)
        course: "",

        // Select
        statusType: "",
        specialityId: "",
        departmentId: "",
        studyformId: "",
        prepodId: ""
      },

      selectedStatusType: {},
      selectedSpecialityId: {},
      selectedDepartmentId: {},
      selectedStudyformId: {},
      selectedPrepodId: {},
      statuses: [],
      specialities: [],
      departments: [],
      studyforms: [],
      prepods: [],

      maxDate: moment()
        .add("years", -25)
        .format("YYYY-MM-DD"),
      minDate: "1930-01-01",

      currentDate: moment().format("YYYY-MM-DD")
    };
  },

  mounted: function() {
    const responseAspirant = axios.get(
      "api/Aspirants/" + this.$route.params.id
    );
    const responseLists = axios.get(
      "api/Aspirants/lists/" + this.$route.params.id
    );

    axios
      .all([responseAspirant, responseLists])
      .then(responses => {
        this.model = responses[0].data;

        // this.statuses = responses[1].data.statuses;
        // this.degrees = responses[1].data.degrees;
        // this.positions = responses[1].data.positions;

        // this.selectedRank = responses[2].data.selectedRank;
        // this.selectedDegree = responses[2].data.selectedDegree;
        // this.selectedPosition = responses[2].data.selectedPosition;
      })
      .catch(err => {
        console.log(err);
      });
  },

  methods: {
    editItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.model.rank = this.selectedRank.value;
          this.model.degree = this.selectedDegree.value;
          this.model.position = this.selectedPosition.value;
          //          this.model.departments = this.departments;
          axios
            .put("api/Prepods/" + this.model.id, this.model)
            .then(response => {
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