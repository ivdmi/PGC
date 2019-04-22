import Vue from "vue";
import VueRouter from "vue-router";
import VeeValidate, { Validator } from "vee-validate";
import uk from "vee-validate/dist/locale/uk";
import BootstrapVue from "bootstrap-vue";
import { ClientTable, Event } from "vue-tables-2";

import App from "./App.vue";

import SpecialityList from "./components/Specialities/SpecialityList.vue";

import AspirantList from "./components/Aspirants/AspirantList.vue";
import AspirantAdd from "./components/Aspirants/AspirantAdd.vue";
import AspirantEdit from "./components/Aspirants/AspirantEdit.vue";

import OrderList from "./components/Orders/OrderList.vue";
import OrderAdd from "./components/Orders/OrderAdd.vue";
import OrderEdit from "./components/Orders/OrderEdit.vue";

import DepartmentList from "./components/Departments/DepartmentList.vue";
import DepartmentAdd from "./components/Departments/DepartmentAdd.vue";
import DepartmentEdit from "./components/Departments/DepartmentEdit.vue";

import FacultyList from "./components/Faculties/FacultyList.vue";
import FacultyAdd from "./components/Faculties/FacultyAdd.vue";
import FacultyEdit from "./components/Faculties/FacultyEdit.vue";

import PrepodList from "./components/Prepods/PrepodList.vue";
import PrepodAdd from "./components/Prepods/PrepodAdd.vue";
import PrepodEdit from "./components/Prepods/PrepodEdit.vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import "vue-good-table/dist/vue-good-table.css";

import bTable from "bootstrap-vue/es/components/table/table";
//import BTable from "bootstrap-vue/es/components/table/table";
import axios from "axios";

Vue.config.productionTip = false;

Vue.use(BootstrapVue);
// Vue.component("b-table", BTable);
Vue.component("b-table", bTable);
Vue.use(bTable);
Vue.use(axios);

Vue.use(ClientTable);
Vue.use(VueRouter);

Validator.localize({ uk: uk });

const phoneRule = {
  getMessage(field, args) {
    return "Поле ${field} має містити телефонний номер";
  },
  validate(value, args) {
    const MOBILREG = /^[+]*\d+[\d-]{5,14}\d+$/;
    return MOBILREG.test(value);
  }
};

const notNull = {
  getMessage(field, args) {
    return "Виберіть поле ${field}";
  },
  validate(value, args) {
    var k = false;
    //if ((value != null) && (value.value != 0) && (value.value != null))
    if (value != null && value.value != null) {
      k = true;
    }
    return k;
  }
};

VeeValidate.Validator.extend("phone", phoneRule);
VeeValidate.Validator.extend("selectValue", notNull);

Vue.use(VeeValidate, { locale: "uk" });

const routes = [
  {
    path: "/",
    component: App,
    children: [
      {
        path: "/specialities",
        component: SpecialityList
      },

      { path: "/aspirants", component: AspirantList },
      {
        path: "aspirant-add",
        name: "AspirantAdd",
        component: AspirantAdd
      },
      {
        path: "aspirant-edit/:id",
        name: "AspirantEdit",
        component: AspirantEdit
      },

      { path: "/orders", component: OrderList },
      {
        path: "order-add",
        name: "OrderAdd",
        component: OrderAdd
      },
      {
        path: "order-edit/:id",
        name: "OrderEdit",
        component: OrderEdit
      },

      {
        path: "faculty-edit/:item",
        name: "FacultyEdit",
        component: FacultyEdit
      },
      {
        path: "/departments",
        component: DepartmentList
      },
      {
        path: "department-add",
        name: "DepartmentAdd",
        component: DepartmentAdd
      },
      {
        // path: "department-edit/:item", name: "DepartmentEdit", component: DepartmentEdit
        path: "department-edit/:id",
        name: "DepartmentEdit",
        component: DepartmentEdit
      },
      {
        path: "/prepods",
        component: PrepodList
      },
      {
        path: "prepod-add",
        name: "PrepodAdd",
        component: PrepodAdd
      },
      {
        path: "prepod-edit/:id",
        name: "PrepodEdit",
        component: PrepodEdit
      }
    ]
  }
];

const router = new VueRouter({
  routes,
  mode: "history"
});

// export const eventBus = new Vue();

new Vue({
  el: "#app",
  template: "<div><router-view></router-view></div>",
  router
});
