<template>
  <div class="col-8 offset-2">
    <router-view></router-view>

    <div>
      <p>
        <button class="btn btn-warning" title="Додати запис" v-on:click="launchItemAdd"> Додати </button>        
      </p>      

      <!-- Модальне вікно помилки видалення -->
      <b-modal class="text-danger border-danger" ref="errorImportRef" title="Помилка">        
        <b-alert show variant="danger">{{this.errorData}}. <br> Помилка видалення запису <br> Можливо, на цей запис є посилання в інших записах</b-alert>
      </b-modal>

      <div>
        <vue-good-table        
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 30,
            perPageDropdown: [20, 30, 50],
            setCurrentPage: 1,
            nextLabel: '',
            prevLabel: '',
            rowsPerPageLabel: 'Строк',
            ofLabel: 'з',
            pageLabel: 'Стор.', // for 'pages' mode
            allLabel: 'All',
          }"
          :search-options="{
            enabled: true
          }">
          row-style-class="font-14"
          styleClass="vgt-table condensed bordered"
          >
           <template slot="table-row" slot-scope="props">
    <span v-if="props.column.field == 'Delete'">
      <button class="btn btn-warning btnxs" @click="removeItem(props.row.id)" title="Видалити запис">X</button>
    </span>
    <span v-else-if="props.column.field == 'Edit'">
      <button class="btn btn-warning btnxs" @click="launchItemEdit(props.row)" title="Редагувати запис">Зм</button>
    </span>
    <span v-else>
      {{props.formattedRow[props.column.field]}}
    </span>
  </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>

import FacultyAdd from "./FacultyAdd";
import FacultyEdit from "./FacultyEdit";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "FacultyList",
  components: {    
    VueGoodTable,    
    FacultyAdd
  },
  data() {
    return {
      list: [], 
      errorData: "",
      columns: [
        {
          label: "Назва",
          field: "name"
        },        
        {
          label: "Скорочення",
          field: "acronym",
        },        
        {
          label: "Декан",
          field: "decan",
        },
        {
          label: "Телефон",
          field: "phone",
        },
        {
          label: "Телефон2",
          field: "phone2",
        },
        {
          label: "Email",
          field: "email",
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
},

  methods: {
    reloadList() {
      var self = this;
      axios.get(`api/Faculties`).then(function(response) {
        self.list = response.data;
      });      
    },
    
        // вызов дочерней формы
    launchItemAdd() {
      this.$router.push("/faculty-add");
    },
    
    launchItemEdit(item) {
      this.$router.push({
        name: "FacultyEdit",
        params: { item: item }
      });      
    },

    removeItem(id) {
      var self = this;
      axios.delete("api/Faculties/" + id).then(response => 
         {
          self.list = self.list.filter(item => {
          return item.id !== id;
        });
      }, 
      (error) =>       
      {
          this.errorData = error.response.data;
          this.$refs.errorImportRef.show();        
      });
    },    
  }
};
</script>