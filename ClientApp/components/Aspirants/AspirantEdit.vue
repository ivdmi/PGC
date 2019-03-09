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
              :class="{ 'has-error': errors.has('surename') }"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('surename')" class="offset-3 alert-validate">
          {{ errors.first("surename") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Ім'я:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="name"
              v-model="model.name"
              v-validate="'required|alpha'"
              :class="{ 'has-error': errors.has('name') }"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('name')" class="offset-3 alert-validate">
          {{ errors.first("name") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">По батькові:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="patronymic"
              v-model="model.patronymic"
              v-validate="'alpha'"
              :class="{ 'has-error': errors.has('patronymic') }"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('patronymic')" class="offset-3 alert-validate">
          {{ errors.first("patronymic") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Телефон:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="text"
              name="Телефон"
              v-model="model.phone"
              v-validate="'phone'"
              :class="{ 'has-error': errors.has('Телефон') }"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Телефон')" class="offset-3 alert-validate">
          {{ errors.first("Телефон") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Email:</b-col>
          <b-col cols="5">
            <b-form-input
              size="sm"
              type="email"
              name="Email"
              v-model="model.email"
              v-validate="'email'"
              :class="{ 'has-error': errors.has('Email') }"
            ></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Email')" class="offset-3 alert-validate">
          {{ errors.first("Email") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Статус:</b-col>
          <b-col cols="5">
            <v-select
              name="Статус"
              label="text"
              :options="statuses"
              v-model="selectedStatusType"
              v-validate="'required|selectValue'"
              :class="{ 'has-error': errors.has('Статус') }"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Статус')" class="offset-3 alert-validate">
          {{ errors.first("Статус") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Форма:</b-col>
          <b-col cols="5">
            <v-select
              name="Форма"
              label="text"
              :options="studyforms"
              v-model="selectedStudyform"
              v-validate="'required|selectValue'"
              :class="{ 'has-error': errors.has('Форма') }"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Форма')" class="offset-3 alert-validate">
          {{ errors.first("Форма") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Кафедра:</b-col>
          <b-col cols="5">
            <v-select
              class="tzSelect"
              name="Кафедра"
              label="text"
              :options="departments"
              v-model="selectedDepartment"
              v-validate="'required|selectValue'"
              :class="{ 'has-error': errors.has('Кафедра') }"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Кафедра')" class="offset-3 alert-validate">
          {{ errors.first("Кафедра") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Керівник:</b-col>
          <b-col cols="5">
            <v-select
              name="Керівник"
              label="text"
              :options="filterPrepods"
              v-model="selectedPrepod"
              v-validate="'required|selectValue'"
              :class="{ 'has-error': errors.has('Керівник') }"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Керівник')" class="offset-3 alert-validate">
          {{ errors.first("Керівник") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Спеціальність:</b-col>
          <b-col cols="5">
            <v-select
              name="Спеціальність"
              label="text"
              :options="specialities"
              v-model="selectedSpeciality"
              v-validate="'required|selectValue'"
              :class="{ 'has-error': errors.has('Спеціальність') }"
            >
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Спеціальність')" class="offset-3 alert-validate">
          {{ errors.first("Спеціальність") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Бюджет (контракт)</b-col>
          <b-col cols="5">
            <span>
              <input
                type="checkbox"
                class="short-check"
                v-model="model.budget"
              />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Докторант (аспірант)</b-col>
          <b-col cols="5">
            <span>
              <input
                type="checkbox"
                class="short-check"
                v-model="model.doctorant"
              />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Чоловік (жінка)</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.sex" />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">В наявності:</b-col>
          <b-col cols="5">
            <span>
              <input
                type="checkbox"
                class="short-check"
                v-model="model.present"
              />
            </span>
          </b-col>
        </b-row>

        <div class="col-10 offset-2">
          <br />
          <b-row class="pad-4">
            <input
              type="submit"
              class="btn btn-warning mr-2"
              value="Зберегти"
            />
            <router-link to="/aspirants" tag="button" class="btn btn-warning">
              Скасувати
            </router-link>
          </b-row>
          <hr />
        </div>
      </b-container>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import vSelect from "vue-select";
import moment from "moment";
//  regex - поиск первого слова до пробельного символа
const firstWordRegex = /(^\S+)/;

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
        studyform: "",

        specialityId: "",
        departmentId: "",
        prepodId: ""
      },

      selectedStatusType: {},
      selectedStudyform: {},

      selectedSpeciality: {},
      selectedDepartment: {},
      selectedPrepod: {},

      statuses: [],
      studyforms: [],

      specialities: [],
      departments: [],
      prepods: [],
      // filterPrepods: [],

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

        this.statuses = responses[1].data.statuses;
        this.studyforms = responses[1].data.studyforms;

        this.specialities = responses[1].data.specialities;
        this.departments = responses[1].data.departments;
        this.prepods = responses[1].data.prepods;

        this.selectedStatusType = responses[1].data.selectedStatus;
        this.selectedStudyform = responses[1].data.selectedStudyform;
        this.selectedSpeciality = responses[1].data.selectedSpeciality;
        this.selectedDepartment = responses[1].data.selectedDepartment;
        this.selectedPrepod = responses[1].data.selectedPrepod;
      })
      .catch(err => {
        console.log(err);
      });
  },

  computed: {
    filterPrepods: function() {
      var prepodList = this.prepods;

      if (this.selectedDepartment.text != null) {
        let dep = this.selectedDepartment.text.match(firstWordRegex);

        // regex - поиск целого слова - акронима кафедры в строке Препода
        let matchWholeWord = new RegExp(
          "(^" + dep[0] + "[^а-яА-Я])|(\\W" + dep[0] + "[^а-яА-Я])"
        );

        prepodList = this.prepods.filter(p => {
          return matchWholeWord.test(p.text);
        });
      }
      return prepodList;
    }
  },

  methods: {
    editItem() {
      // ВАЛИДАЦИЯ ПРЕПОД - КАФЕДРА

      let dep = this.selectedDepartment.text.match(firstWordRegex);

      // regex - поиск целого слова - акронима кафедры в строке Препода
      let matchWholeWord = new RegExp(
        "(^" + dep[0] + "[^а-яА-Я])|(\\W" + dep[0] + "[^а-яА-Я])"
      );

      //      if (!matchWholeWord.test(selectedPrepod.text))
      var t = this.errors;

      this.errors.add({
        scope: "DONT USE SCOPE IF YOU DONT HAVE",
        field: "email",
        msg: "email is already in use"
      });

      var field = this.$validator.fields.find({ name: "email" }); // get me the first field that has this name

      field.setFlags({
        valid: false,
        dirty: true
      });

      // this.errors.push(new { field: patronymic, message: "dfs" }());
      //    this.errors.add("patronymic", "message"); //this message i want to move to the dictionary

      // if (selectedPrepod)
      this.$validator.validate().then(valid => {
        if (valid) {
          this.model.statusType = this.selectedStatusType.value;
          this.model.studyform = this.selectedStudyform.value;

          this.model.prepodId = this.selectedPrepod.value;
          this.model.departmentId = this.selectedDepartment.value;
          this.model.specialityId = this.selectedSpeciality.value;
          axios
            .put("api/Aspirants/" + this.model.id, this.model)
            .then(response => {
              this.$router.push("/Aspirants");
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