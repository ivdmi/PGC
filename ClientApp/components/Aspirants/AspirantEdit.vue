<template>
  <div class="col-10 offset-1">
    <h2>Редагування облікового запису аспіранта</h2>
    <form v-on:submit.prevent="editItem">
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

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">В наявності:</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.present" />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Академвідпустка:</b-col>
          <b-col cols="5">
            <span>
              <input type="checkbox" class="short-check" v-model="model.vacation" />
            </span>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="2" class="text-right">Відмітки:</b-col>
          <b-col cols="8">
            <label class="text-right col-1">P1</label>
            <label class="text-right col-1">P2</label>
            <label class="text-right col-1">P3</label>
            <label class="text-right col-1">P4</label>
            <label class="text-right col-1">P5</label>
            <label class="text-right col-1">P6</label>
            <label class="text-right col-1">P7</label>
          </b-col>
        </b-row>
        <b-row class="offset-2 col-8">
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p1" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p2" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p3" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p4" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p5" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p6" />
          </span>
          <span class="col-1">
            <input type="checkbox" class="short-check" v-model="model.p7" />
          </span>
        </b-row>

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
        vacation: "",
        p1: "",
        p2: "",
        p3: "",
        p4: "",
        p5: "",
        p6: "",
        p7: "",

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

      errorMessage: "",

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
      var prepodList = [];

      if (this.selectedDepartment.text != null) {
        // акроним кафедры
        let dep = this.selectedDepartment.text.match(firstWordRegex);

        // let matchWholeWord = new RegExp("(^|\\s)" + dep[0] + "(?=\\s|$)", "g");

        // Cообщение об ошибке, если кафедра и препод не совпадают
        {
          if (
            this.selectedPrepod.сomment != null &&
            this.selectedPrepod.сomment.indexOf(dep[0]) == -1
          ) {
            this.errorMessage =
              "Викладач не працює на обраній кафедрі (в інституті)";
          } else {
            this.errorMessage = "";
          }
        }

        prepodList = this.prepods.filter(p => {
          let result = false;

          if (
            p.сomment != null &&
            (p.сomment == dep[0] ||
              p.сomment.indexOf(", " + dep[0]) != -1 ||
              p.сomment.indexOf(dep[0] + ",") != -1)
          ) {
            result = true;
          }

          return result;
          // matchWholeWord.test(p.сomment);
        });
      }
      return prepodList;
    }
  },

  methods: {
    editItem() {
      this.$validator.validate().then(valid => {
        if (valid && this.errorMessage === "") {
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
    }
  }
};
</script>