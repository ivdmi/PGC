<template>
  <div class="col-10 offset-1">
    <h2>Редагувати наказ</h2>
    <form v-on:submit.prevent="editItem">
      <b-container fluid>
        <b-row class="pad-4">
          <b-col cols="3" class="text-right">Номер:</b-col>
          <b-col cols="9">
            <b-form-input size="sm" name="number" v-model="model.number" v-validate="'required|numeric'" :class="{'has-error': errors.has('number')}"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('number')" class="offset-3 alert-validate">{{ errors.first('number') }}</div>

        <b-row class="pad-4">
          <b-col cols="3" class="text-right">Дата наказу:</b-col>
          <b-col cols="9">
            <b-form-input name="Дата" type="date" v-model="model.date" size="sm" min="2019-01-01" :max=this.currentDate v-validate="{
              required,
              date_format : 'YYYY-MM-DD',
              date_between : ['2019-01-01', this.currentDate, true]
            }" :class="{'has-error': errors.has('Дата')}"></b-form-input>
          </b-col>
        </b-row>
        <div v-if="errors.has('Дата')" class="offset-3 alert-validate">{{ errors.first('Дата') }}</div>

        <b-row class="pad-4">
          <b-col cols="3" class="text-right">Зміст:</b-col>
          <b-col cols="9">
            <b-form-input v-model="model.context" size="sm"></b-form-input>
          </b-col>
        </b-row>

        <b-row class="pad-4">
          <b-col cols="3" class="text-right">Вид наказу:</b-col>
          <b-col cols="9">
            <v-select name="Вид" label="text" :options="orderTypes" v-model="selectedOrderType" v-validate="'required|selectValue'" :class="{ 'has-error': errors.has('Вид') }">
              <template slot="option" slot-scope="option">
                <span v-html="option.text"></span>
              </template>
            </v-select>
          </b-col>
        </b-row>
        <div v-if="errors.has('Вид')" class="offset-3 alert-validate">
          {{ errors.first("Вид") }}
        </div>

        <div class="col-10 offset-2">
          <br />
          <b-row class="pad-4">
            <!-- <input v-if="submitVisible" type="submit" class="btn btn-warning mr-2" value="Зберегти" /> -->
            <input type="submit" class="btn btn-warning mr-2" value="Зберегти" />
            <router-link to="/orders" tag="button" class="btn btn-warning">Скасувати</router-link>
          </b-row>
          <hr />
        </div>

        <div class="col-10 offset-2">
          <h5>Аспіранти, включені до наказу</h5>
          <order-aspirant-selected-list @del-aspirant="onDel" :list="this.aspirantsInOrder"></order-aspirant-selected-list>
          <hr>
          <h5>Обрати аспірантів</h5>
          <order-aspirant-list @add-aspirant="onAddOne" @add-aspirants="onAddSeveral" :list="filterAspirants"></order-aspirant-list>
        </div>

      </b-container>
    </form>
  </div>

</template>

<script>
import axios from "axios";
import vSelect from "vue-select";
import moment from "moment";
import orderAspirantList from "./OrderAspirantList";
import orderAspirantSelectedList from "./OrderAspirantSelectedList";

export default {
  components: {
    vSelect,
    axios,
    orderAspirantList,
    orderAspirantSelectedList
  },

  data() {
    return {
      model: {
        id: "",
        number: "",
        context: "",
        orderTypeValue: "",
        date: "",
        aspirantIdList: [],
        budget: "",
        studyForm: ""
      },

      aspirantsInOrder: [],
      aspirants: [],

      selectedOrderType: {},
      selectedStudyForm: {},

      orderTypes: [],
      //      studyForms: [],

      //    studyFormVisible: false,
      // submitVisible: true,

      currentDate: moment().format("YYYY-MM-DD")
    };
  },

  mounted: function() {
    const responseOrder = axios.get("api/Orders/" + this.$route.params.id);
    const responseAspirantsInOrder = axios.get(
      "api/Orders/aspirants/" + this.$route.params.id
    );
    const responseAspirants = axios.get("api/Aspirants");
    const responseOrderTypes = axios.get("api/Orders/types");
    //    const responseStudyForms = axios.get("api/Aspirants/sforms");

    axios
      .all([
        responseOrder,
        responseAspirantsInOrder,
        responseAspirants,
        responseOrderTypes
        //     responseStudyForms
      ])
      .then(responses => {
        this.model = responses[0].data;
        this.aspirantsInOrder = responses[1].data;
        this.aspirants = responses[2].data;
        this.orderTypes = responses[3].data;
        //      this.studyForms = responses[4].data;
        this.selectedOrderType.value = responses[0].data.orderType;
        this.selectedOrderType.text = this.orderTypes.find(
          x => x.value === this.selectedOrderType.value
        ).text;

        // Если тип - "Зміна форми навчання" - менять не будем
        // if (this.selectedOrderType.value == 7) {
        //   this.submitVisible = false;
        // }

        // if (this.aspirantsInOrder.length != 0) {
        //   this.selectedStudyForm.value = this.aspirantsInOrder[0].studyForm;
        //   this.selectedStudyForm.text = this.studyForms.find(
        //     x => x.value === this.selectedStudyForm.value
        //   ).text;
        //   this.model.budget = this.aspirantsInOrder[0].budget;
        // }
      })
      .catch(err => {
        console.log(err);
      });
  },

  computed: {
    // список аспірантів та форма залежно від типу наказу
    filterAspirants: function() {
      //    this.studyFormVisible = false;
      var aspirantList = this.aspirants;
      var aspirantListActiv = this.aspirants;

      if (this.selectedOrderType.value == 1) {
        // Зарахування
        aspirantList = this.aspirants.filter(item => {
          return item.course == 0;
        });
      } else {
        aspirantListActiv = this.aspirants.filter(item => {
          return item.course != 0 && item.statusType == "навчання";
        });
        switch (this.selectedOrderType.value) {
          case 6: {
            // Повернення з відпустки
            aspirantList = this.aspirants.filter(item => {
              return item.vacation == "+";
            });
            break;
          }
          case 7: {
            // Зміна форми навчання
            //          this.studyFormVisible = true;
            break;
          }
          case 8: {
            // Інший
            aspirantList = this.aspirants;
            break;
          }
          default: {
            aspirantList = aspirantListActiv;
            break;
          }
        }
      }

      //     this.aspirantsInOrder = [];

      return aspirantList;
    }
  },

  methods: {
    editItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.model.orderTypeValue = this.selectedOrderType.value;
          this.model.studyForm = this.selectedStudyForm.value;
          // выбрать только Id
          this.model.aspirantIdList = this.aspirantsInOrder.map(function(v) {
            return v.id;
          });
          // axios.put("api/orders/", this.model).then(response => {
          //   this.$router.push("/orders");
          // });

          // axios.put("api/orders/", this.model).then(response => {
          //   this.$router.push("/orders");
          // });

          axios
            .put("api/Orders/" + this.model.id, this.model)
            .then(response => {
              this.$router.push("/orders");
            });
        }
      });
    },

    onDel(data) {
      // удалить строку из aspirantsInOrder - верхний список
      var k = this.aspirantsInOrder
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (k != -1) {
        this.aspirantsInOrder.splice(k, 1);
      }

      // Зарахування
      if (this.selectedOrderType.value == 1) {
        var item = this.aspirants.find(x => x.id === data.id);
        if (item != null) {
          item.course = 0;
        }
      }

      // добавить строку в aspirants - нижний список
      var m = this.aspirants
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (m === -1) {
        this.aspirants.push(data);
      }
    },

    onAddOne(data) {
      // добавить строку в aspirantsInOrder - верхний список
      var k = this.aspirantsInOrder
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (k === -1) {
        this.aspirantsInOrder.push(data);
      }

      // удалить строку из aspirants - нижний список
      var n = this.aspirants
        .map(function(e) {
          return e.id;
        })
        .indexOf(data.id);
      if (n != -1) {
        this.aspirants.splice(n, 1);
      }
    },

    onAddSeveral(data) {
      data.forEach(element => {
        var k = this.aspirantsInOrder
          .map(function(e) {
            return e.id;
          })
          .indexOf(element.id);
        if (k === -1) {
          this.aspirantsInOrder.push(element);
        }

        var n = this.aspirants
          .map(function(e) {
            return e.id;
          })
          .indexOf(element.id);
        if (n != -1) {
          this.aspirants.splice(n, 1);
        }
      });
    },

    // Возвращаем данные (список аспирантов) из подчиненной формы
    // метод onAdd вызывается по событию add-order
    onAdd(data) {
      this.aspirantsInOrder = data;
    }
  }
};
</script>