import Vue from "vue";
import VueRouter from "vue-router";
import VeeValidate, { Validator } from "vee-validate";
import uk from "vee-validate/dist/locale/uk"
import BootstrapVue from "bootstrap-vue";
import { ServerTable, ClientTable, Event } from "vue-tables-2";
import App from "./App.vue";
import TodoList from "./components/Todo/TodoList.vue";
import SpecialityList from "./components/Specialities/SpecialityList.vue";

import AspirantList from "./components/Aspirants/AspirantList.vue";
import AspirantAdd from "./components/Aspirants/AspirantAdd.vue";

import ProductList from "./components/Products/Products.vue";
import AddProduct from "./components/Products/AddProduct.vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";
import bTable from "bootstrap-vue/es/components/table/table";


Vue.config.productionTip = false;
Vue.use(BootstrapVue);
Vue.component("b-table", bTable);
Vue.use(bTable);
Vue.use(ClientTable);
Vue.use(VueRouter);

Validator.localize({uk: uk});
Vue.use(VeeValidate, { locale: 'uk'});
// VeeValidate.Validator.extend('phone', phoneRule);

const phoneRule = {
  
  getMessage(field, args){
    return 'Поле ${field} має містити телефонний номер';
  },
  
  validate(value, args) {
  const MOBILREG = /^((1[3578][0-9])+\d{8})$/;
  return MOBILREG.test(value);
}
}

// const dictionary = {
//   en: {
//     messages:{
//       alpha: () => 'Some English Message'
//     }
//   },
//   us: {
//     messages: {
//       alpha: 'Введіьб'
//     }
//   }
// };

// Override and merge the dictionaries
// Validator.localize('uk');

// const validator = new Validator({ first_name: 'alpha' });

// validator.localize('uк'); // now this validator will generate messages in Arabic.


const routes = [
  {
    path: "/",
    component: App,
    children: [
      {
        path: "/todo",
        component: TodoList
      },
      {
        path: "/specialities",
        component: SpecialityList
      },
      {
        path: "/aspirants",
        component: AspirantList,
        children: [
          { path: "aspirant-add", name: "AspirantAdd", component: AspirantAdd }
          // {
          //   name: "Edit",
          //   path: "edit/:id",
          //   component: EditItem
          // }
        ]
      },
      {
        path: "/aspirant-add",
        component: AspirantAdd
      },
      {
        path: "/products",
        component: ProductList,
        children: [
          { path: "add-product", component: AddProduct }
          // {
          //   name: "Edit",
          //   path: "edit/:id",
          //   component: EditItem
          // }
        ]
      }
    ]
  }
];

const router = new VueRouter({
  routes,
  mode: "history"
});

export const eventBus = new Vue();

new Vue({
  el: "#app",
  template: "<div><router-view></router-view></div>",
  router
});
