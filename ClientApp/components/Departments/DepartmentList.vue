<template>
  <div class="col-10 offset-1">
    <router-view></router-view>

    <div>
      <div class="row" >
        <div class="col-2"><button class="btn btn-warning" title="Додати запис" v-on:click="launchItemAdd"> Нова кафедра</button></div>        
        <div class="col-2">
          <label class="btn btn-warning col-12">
            Імпорт з xls <input type="file" multiple="false" id="sheetjs-input" accept=".xls,.xlsx" @change="importExcel" style="display:none">
          </label>
        </div>
      
      <!-- Назва файлу, що завантажувався -->
      <span class='label label-info' id="upload-file-info"></span>
      
      <!-- Модальне вікно помилки імпорту -->
      <b-modal class="text-danger border-danger" ref="errorImportRef" title="Помилка імпорту з XLS файлу!">        
        <b-alert show variant="danger">{{this.errorData}}</b-alert>
      </b-modal>
      </div>
      
      <div>
        <vue-good-table        
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 150,
            perPageDropdown: [5, 20, 150],
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
import DepartmentAdd from "./DepartmentAdd";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";
import XLSX from "xlsx";

export default {
  name: "DepartmentList",
  components: {
    VueGoodTable,
    DepartmentAdd,
    XLSX
  },
  data() {
    return {
      list: [],
      errorData: "",
      columns: [
        {
          label: "Кафедра",
          field: "name"
        },
        {
          label: "Каф",
          field: "acronym"
        },
        {
          label: "Фак",
          field: "faculty.acronym"
        },
        {
          label: "Факультет",
          field: "faculty.name"
        },
        {
          label: "Спеціальності",
          field: "specialitiesString"
        },
        {
          label: "Декан",
          field: "decan"
        },
        {
          label: "Телефон",
          field: "phone"
        },
        {
          label: "Телефон2",
          field: "phone2"
        },
        {
          label: "Email",
          field: "email"
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
    // загрузить список
    this.reloadList();

    // Спеціальності
    // axios.get(`api/specialities/names`).then(response => {
    //   this.specialities = response.data;

    // });
  },

  methods: {
    reloadList() {
      var self = this;
      axios.get(`api/Departments`).then(function(response) {
        self.list = response.data;
      });
    },

    // вызов дочерней формы
    launchItemAdd() {
      this.$router.push("/department-add");

      // this.$router.push({
      //   name: "DepartmentAdd",
      //   params: {
      //     spec: this.specialities
      //   }
      // });
    },

    launchItemEdit(item) {
      this.$router.push({
        name: "DepartmentEdit",
        params: { id: item.id }
        // params: { item: item }
      });
    },

    removeItem(id) {
      var self = this;
      axios.delete("api/Departments/" + id).then(function(response) {
        self.list = self.list.filter(item => {
          return item.id !== id;
        });
      });
    },

    importExcel: function(evt) {
      var file;
      var files = evt.target.files;

      if (!files || files.length == 0) return;

      file = files[0];

      // очистить input, чтобы можно было выбрать тот же файл повторно
      document.getElementById("sheetjs-input").value = "";

      var reader = new FileReader();

      var self = this;

      const importPromise = new Promise((resolve, reject) => {
        reader.onload = function(e) {
          // pre-process data
          var binary = "";
          var bytes = new Uint8Array(e.target.result);
          var length = bytes.byteLength;
          for (var i = 0; i < length; i++) {
            binary += String.fromCharCode(bytes[i]);
          }

          /* read workbook */
          var wb = XLSX.read(binary, { type: "binary" });

          /* grab first sheet */
          var wsname = wb.SheetNames[0];
          var ws = wb.Sheets[wsname];

          // var stream = XLSX.stream.to_json(ws, { raw: true });

          let Json = XLSX.utils.sheet_to_json(ws);

          // axios.post(`api/departments/import`, JsonDepartments)
          // .then(response => resolve("result"))
          // .catch(err => reject(new Error("Что-то пошло не так!")));

          axios
            .post(`api/departments/import`, Json)
            .then(response => resolve("result"))
            .catch(err => {
              reject(new Error(err.response.data));
            });

          /* generate HTML */
          // var HTML = XLSX.utils.sheet_to_html(ws);

          /* update table  - вывод htms в объект */
          //document.getElementById("out-table").innerHTML = HTML;
          document.getElementById("upload-file-info").textContent = file.name;

          /* show export button */
          //document.getElementById("export-table").style.visibility = "visible";
        };

        reader.readAsArrayBuffer(file);
      });

      importPromise.then(
        result => this.reloadList(),
        error => {
          this.errorData = error.message;
          this.$refs.errorImportRef.show();
        }
      );
    }

  }
};
</script>