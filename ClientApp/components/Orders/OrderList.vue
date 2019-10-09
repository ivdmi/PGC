<template>
  <div class="col-8 offset-2">
    <router-view></router-view>

    <div>
      <div class="col-2">
        <button class="btn btn-warning" title="Додати запис" v-on:click="addItem">Новий наказ</button>
      </div>

      <div>
        <vue-good-table :columns="columns" :rows="list" :line-numbers="true" :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 20,
            perPageDropdown: [5, 20, 50],
            setCurrentPage: 1,
            nextLabel: '',
            prevLabel: '',
            rowsPerPageLabel: 'Строк',
            ofLabel: 'з',
            pageLabel: 'Стор.', // for 'pages' mode
            allLabel: 'All',
          }" row-style-class="font-14" styleClass="vgt-table condensed bordered">

          <template slot="table-row" slot-scope="props">
            <span v-if="props.column.field == 'Delete' && props.row.orderType != 'Зміна форми навчання'">
              <button class="btn btn-warning btnxs" @click="removeItem(props.row)" title="Видалити запис">X</button>
            </span>
            <span v-else-if="props.column.field == 'Edit' && props.row.orderType != 'Зміна форми навчання'">
              <button class="btn btn-warning btnxs" @click="editItem(props.row)" title="Редагувати запис">Зм</button>
            </span>
            <span v-else>{{props.formattedRow[props.column.field]}}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
// import OrderAdd from "./OrderAdd";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "OrderList",
  components: {
    VueGoodTable
  },
  data() {
    return {
      list: [],

      columns: [
        {
          label: "Номер",
          field: "number",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Дата",
          field: "date",
          type: "date",
          dateInputFormat: "YYYY-MM-DD",
          dateOutputFormat: "DD.MM.YYYY"
        },
        {
          label: "Зміст",
          field: "context",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Вид",
          field: "orderType",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Del",
          field: "Delete"
        },
        {
          label: "Ed",
          field: "Edit"
        }
      ]
    };
  },

  mounted: function() {
    //   debugger;
    axios.get(`api/Orders`).then(response => {
      this.list = response.data;
    });
  },

  methods: {
    // -------------- Добавить --------------
    addItem() {
      this.$router.push("/order-add");
    },

    // -------------- Редактировать --------------
    editItem(item) {
      this.$router.push({
        name: "OrderEdit",
        params: { id: item.id }
      });
    },

    // -------------- Удалить --------------
    removeItem(item) {
      if (item.orderType != "Зміна форми навчання") {
        axios.delete("api/Orders/" + item.id).then(response => {
          this.list = this.list.filter(i => {
            return i.id !== item.id;
          });
        });
      }
    }
  }
};
</script>