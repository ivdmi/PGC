<template>
  <div class="col-8 offset-2">
    <router-view></router-view>

    <div>
      <p>
        <button class="btn btn-warning" title="Додати запис" v-on:click="launchItemAdd"> Додати </button>        
      </p>      

      <div>
        <vue-good-table        
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :pagination-options="{
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
          }"
          row-style-class="font-14"
          styleClass="vgt-table condensed bordered"
          >  

  <!-- <template slot="table-row" slot-scope="props">
    <span v-if="props.column.field == 'Delete'">
      <button class="btn btn-warning btnxs" @click="removeItem(props.row.id)" title="Видалити запис">X</button>
    </span>
    <span v-else-if="props.column.field == 'Edit'">
      <button class="btn btn-warning btnxs" @click="launchItemEdit(props.row)" title="Редагувати запис">Зм</button>
    </span>
    <span v-else>
      {{props.formattedRow[props.column.field]}}
    </span>
  </template> -->
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
import OrderAdd from "./OrderAdd";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "OrderList",
  components: {    
    VueGoodTable,    
    OrderAdd
  },
  data() {
    return {
      list: [],
      // отображение связанных полей
      otderTypes: [],

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
          field: "ordertypeId",
          filterable: true,
          formatFn: this.formatTypeFn
        },        
        {
          label: "Del",
          field: "Delete"
        },
        {
          label: "Ed",
          field: "Edit"
        }
      ],
    };
  },

  mounted: function() {
    // загрузить список
    this.reloadList();

    // ---------- Загрузка связанных таблиц и установка комбобоксов фильтра ----------

    // Типи
    axios.get(`api/Orders/types`).then(response => {
      this.otderTypes = response.data;
      // Установить в комбобокс фильтра список типов
      this.$set(this.columns[3], "filterOptions", {
        enabled: true,
        filterDropdownItems: this.otderTypes,
        placeholder: "Всі"
      });
    });
},

  methods: {
    reloadList() {
      var self = this;
      axios.get(`api/Orders`).then(function(response) {
        self.list = response.data;
      });      
    },
    
  // вызов дочерней формы
    launchItemAdd() {
      this.$router.push({
        name: "OrderAdd",
        params: { 
          types: this.otderTypes          
        }
      });
    },

    // // вызов дочерней формы
    // launchItemEdit(item) {
    //   // скрыть список и кнопку - оставить только дочернюю форму
    //   this.isVisible = false;
    //   // var aspirant = item;
    //   this.$router.push({
    //     name: "AspirantEdit",
    //     params: {
    //       item: item,
    //       specialities: this.specialities,
    //       statuses: this.statuses
    //     }
    //   });
    // },

    formatTypeFn: function(value) {
      var status = this.otderTypes.find(x => x.value === value);
      return status.text;
    }
  }
};
</script>