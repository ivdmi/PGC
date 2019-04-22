<template>
  <div>
    <h2>Додати аспіранта</h2>
    <form v-on:submit.prevent="addItem">
      <b-container fluid>
        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Прізвище:</b-col>
          <b-col cols="5">
            <b-form-input size="sm" type="text" name="surename" v-model="model.surename" v-validate="'required|alpha'" :class="{ 'has-error': errors.has('surename') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('surename')" class="offset-3 alert-validate">
          {{ errors.first("surename") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Ім'я:</b-col>
          <b-col cols="5">
            <b-form-input size="sm" type="text" name="name" v-model="model.name" v-validate="'required|alpha'" :class="{ 'has-error': errors.has('name') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('name')" class="offset-3 alert-validate">
          {{ errors.first("name") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">По батькові:</b-col>
          <b-col cols="5">
            <b-form-input size="sm" type="text" name="patronymic" v-model="model.patronymic" v-validate="'alpha'" :class="{ 'has-error': errors.has('patronymic') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('patronymic')" class="offset-3 alert-validate">
          {{ errors.first("patronymic") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Телефон:</b-col>
          <b-col cols="5">
            <b-form-input size="sm" type="text" name="Телефон" v-model="model.phone" v-validate="'phone'" :class="{ 'has-error': errors.has('Телефон') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Телефон')" class="offset-3 alert-validate">
          {{ errors.first("Телефон") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Email:</b-col>
          <b-col cols="5">
            <b-form-input size="sm" type="email" name="Email" v-model="model.email" v-validate="'email'" :class="{ 'has-error': errors.has('Email') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Email')" class="offset-3 alert-validate">
          {{ errors.first("Email") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Народження:</b-col>
          <b-col cols="5">
            <b-form-input name="Народження" type="date" v-model="model.birthday" size="sm" :min="this.minDate" :max="this.maxDate" v-validate="{
                date_format: 'YYYY-MM-DD',
                date_between: [this.minDate, this.maxDate, true]
              }" :class="{ 'has-error': errors.has('Народження') }"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Народження')" class="offset-3 alert-validate">
          {{ errors.first("Народження") }}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Статус:</b-col>
          <b-col cols="5">
            <v-select name="Статус" label="text" :options="statuses" v-model="selectedStatusType" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Статус') }">
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
            <v-select name="Форма" label="text" :options="studyforms" v-model="selectedStudyform" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Форма') }">
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
            <v-select class="tzSelect" name="Кафедра" label="text" :options="departments" v-model="selectedDepartment" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Кафедра') }">
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
            <v-select name="Керівник" label="text" :options="filterPrepods" v-model="selectedPrepod" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Керівник') }">
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Керівник')" class="offset-3 alert-validate">
          {{ errors.first("Керівник") }}
        </div>
        <div class="offset-3 alert-validate">
          {{ errorMessage}}
        </div>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Спеціальність:</b-col>
          <b-col cols="5">
            <v-select name="Спеціальність" label="text" :options="specialities" v-model="selectedSpeciality" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Спеціальність') }">
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
              <input type="checkbox" class="short-check" v-model="model.budget" />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Докторант (аспірант)</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.doctorant" />
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

        <!-- <b-row class="pad-4">
          <b-col cols="2" class="text-right">В наявності:</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.present" />
            </span>
          </b-col>
        </b-row> -->

        <div class="col-10 offset-2">
          <br />
          <b-row class="pad-4">
            <input type="submit" class="btn btn-warning mr-2" value="Зберегти" />
            <router-link to="/aspirants" tag="button" class="btn btn-warning">
              Скасувати
            </router-link>
          </b-row>
          <hr />
        </div>
      </b-container>

    </form>

    <hr>
  </div>
</template>

<script>
import axios from "axios";
import vSelect from "vue-select";
import moment from "moment";

const firstWordRegex = /(^\S+)/;

export default {
  components: {
    axios,
    vSelect
  },

  data() {
    return {
      model: {
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
        // stationary: "",
        budget: true,
        doctorant: false,
        sex: true,
        protection: false,
        present: true,

        // int (numeric)
        course: 0,

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

      errorMessage: "",

      maxDate: moment()
        .add("years", -25)
        .format("YYYY-MM-DD"),
      minDate: "1930-01-01",

      currentDate: moment().format("YYYY-MM-DD")
    };
  },

  mounted: function() {
    const responseLists = axios.get(
      "api/Aspirants/lists/" + this.$route.params.id
    );

    axios
      .all([responseLists])
      .then(responses => {
        this.statuses = responses[0].data.statuses;
        this.studyforms = responses[0].data.studyforms;

        this.specialities = responses[0].data.specialities;
        this.departments = responses[0].data.departments;
        this.prepods = responses[0].data.prepods;
        this.selectedStatusType.value = 1;
        this.selectedStudyform.value = 1;

        this.selectedStatusType.text = this.statuses.find(
          x => x.value === 1
        ).text;
        this.selectedStudyform.text = this.studyforms.find(
          x => x.value === 1
        ).text;
      })
      .catch(err => {
        console.log(err);
      });
  },

  computed: {
    filterPrepods: function() {
      var prepodList = this.prepods;

      if (this.selectedDepartment.text != null) {
        // акроним кафедры
        let dep = this.selectedDepartment.text.match(firstWordRegex);

        // regex - поиск целого слова - акронима кафедры в строке Препода
        let matchWholeWord = new RegExp(
          "(^" + dep[0] + "[^а-яА-Я])|(\\W" + dep[0] + "[^а-яА-Я])"
        );

        // Cообщение об ошибке, если кафедра и препод не совпадают
        {
          if (this.selectedPrepod.text.indexOf(dep[0]) == -1) {
            this.errorMessage =
              "Викладач не працює на обраній кафедрі (в інституті)";
          } else {
            this.errorMessage = "";
          }
        }

        prepodList = this.prepods.filter(p => {
          return matchWholeWord.test(p.text);
        });
      }
      return prepodList;
    }
  },

  methods: {
    addItem() {
      this.$validator.validate().then(valid => {
        if (valid && this.errorMessage === "") {
          this.model.statusType = this.selectedStatusType.value;
          this.model.studyform = this.selectedStudyform.value;

          this.model.prepodId = this.selectedPrepod.value;
          this.model.departmentId = this.selectedDepartment.value;
          this.model.specialityId = this.selectedSpeciality.value;
          axios.post("api/Aspirants", this.model).then(response => {
            this.$router.push("/Aspirants");
          });
        }
      });
    }
  }
};
</script>