<template>
  <div>
    <router-view></router-view>

    <div v-show="isVisible">
      <p>
        <!-- <router-link class="btn btn-warning btn-xs" to="/aspirants/aspirant-add">Создать</router-link>     -->
        <button class="btn btn-warning" v-on:click="launchAspirantAdd">Додати нового</button>
      </p>

      <!-- <h3>Old School</h3>
        <ul v-if="list.length">
            <AspirantItem
                v-for="item in list"
                :key="item.id"
                :item="item"
                @remove="removeItem"
            />
        </ul>
        <p v-else>
            Nothing left in the list. Add a new Aspirant in the input above.
      </p>-->
      <div>
        <v-client-table :columns="columns" :data="list" :options="options"></v-client-table>
      </div>
    </div>
  </div>
</template>

<script>
// import AspirantItem from "./AspirantItem";
import AspirantAdd from "./AspirantAdd";
import { eventBus } from "./../../index.js";
import axios from "axios";

export default {
  name: "AspirantList",
  components: {
    //  AspirantItem,
    AspirantAdd
  },
  data() {
    return {
      list: [],      
      columns: [
        "surename",
        "name",
        "patronymic",
        "doctorant",
        "birthday",
        "specialityId"
      ],
      isVisible: true,

      options: {
        headings: {
          surename: "Прізвище",
          name: "Ім'я",
          patronymic: "По батькові",
          doctorant: "Докторант",
          birthday: "Дата Народження",
          specialityId: "Код спеціальності"
        },
        sortable: ["surename", "name", "specialityId"],
        filterable: ["surename", "name", "specialityId"]
      }
    };
  },

  created: function() {
    // загрузить список
    this.reloadList();
    var self = this;

    // инициализировать шину событий
    eventBus.$on("add-aspirant", data => {
      if (data.action == "create") {
        this.addItem(data);
      }
      this.isVisible = true;
    });
  },

  // разрушить шину событий, чтобы не срабатывал повторно
  destroyed: function() {
    eventBus.$off();
  },

  methods: {
    reloadList() {
      var self = this;
      axios.get(`api/Aspirants`).then(function(response) {
        self.list = response.data;
      });
      // .catch(function(e) {
      //   self.errors.push(e)
      // });
    },

    // вызов дочерней формы
    launchAspirantAdd() {
      // скрыть список и кнопку - оставить только дочернюю форму
      this.isVisible = false;
      this.$router.push("/aspirants/aspirant-add");
    },

    addItem(item) {
      var self = this;
      axios
        .post("api/aspirants", item)
        .then(response => {
          item.id = response.data;
          // добавить в список нового аспиранта
          self.list.push(item);
          return response;
        })
        .catch(error => {
          console.log(error);
          return error;
        });
    },

    removeItem(idToRemove) {
      var self = this;
      axios.delete("api/Aspirants/" + idToRemove).then(function(response) {
        self.list = self.list.filter(item => {
          return item.id !== idToRemove;
        });
      });
    }
  }
};
</script>