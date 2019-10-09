<template>
  <div>

    <div class="row">
      <div class="col-2">
        <button class="btn btn-warning" title="Додати запис" v-on:click="addItem">Новий аспірант</button>
      </div>
      <div class="col-2">
        <label class="btn btn-warning col-12">
          Імпорт з xls
          <input type="file" multiple="false" id="sheetjs-input" accept=".xls, .xlsx" @change="importExcel" style="display:none">
        </label>
      </div>
      <span class="label" id="upload-file-info"></span>

      <b-modal class="text-danger border-danger" ref="errorImportRef" title="Помилка імпорту з XLS файлу!">        
        <b-alert show variant="danger"><span v-html="this.errorData"></span></b-alert>
      </b-modal>
    </div>

    <b-table responsive hover striped bordered small :items="list" :fields="fields" class="table-14">

      <template slot="index" slot-scope="data">
        {{ data.index + 1 }}
      </template>

      <template slot="HEAD_selected" slot-scope="data">
        <b-form-checkbox @click.native.stop v-model="allSelected">
        </b-form-checkbox>
      </template>

      <template slot="info" slot-scope="row">
        <b-button size="sm" @click="row.toggleDetails" class="btn btn-info btnxs">
          {{ row.detailsShowing ? 'Сховати' : 'Інфо' }}
        </b-button>
      </template>

      <template slot="delete" slot-scope="row">
        <b-button size="sm" @click="removeItem(row.item.id)" class="btn btn-warning btnxs">
          X
        </b-button>
      </template>

      <template slot="edit" slot-scope="row">
        <b-button size="sm" @click="editItem(row.item)" class="btn btn-warning btnxs" title="Редагувати запис">
          Зм
        </b-button>
      </template>

      <template slot="row-details" slot-scope="row">
        <b-card>
          <p>Телефон: {{row.item.phone}}</p>
          <p>e-mail: {{row.item.email}}</p>
          <!-- <ul>
            <li v-for="(value, key) in row.item" :key="key">{{ key }}: {{ value }}</li>
          </ul> -->
        </b-card>
      </template>

    </b-table>
  </div>
</template>

<script>
import axios from "axios";
// import { VueGoodTable } from "vue-good-table";
import XLSX from "xlsx";

export default {
  name: "AspirantList",
  components: {
    // VueGoodTable,
    XLSX
    //    AspirantAdd,
    //    AspirantEdit
  },
  data() {
    return {
      list: [],
      errorData: "",

      fields: [
        { label: "№", key: "index" },
        { label: "v", key: "HEAD_selected" },
        // {
        //   key: "surename",
        //   label: "Прізвище",
        //   sortable: true,
        //   variant: "info"
        // },
        { key: "surename", label: "Прізвище", sortable: true },
        { key: "name", label: "Ім'я" },
        { key: "patronymic", label: "По батькові" },
        { key: "specialityId", label: "Сп", sortable: true },
        { key: "prepod", label: "Керівник", sortable: true },
        { key: "department", label: "Каф", sortable: true },
        { key: "faculty", label: "Фак", sortable: true },
        //{ key: "studyForm", label: "Форма", sortable: true, variant: "danger"},
        { key: "studyForm", label: "Форма", sortable: true },
        { key: "budget", label: "Б", sortable: true },
        { key: "statusType", label: "Статус", sortable: true },
        { key: "course", label: "К", sortable: true },
        { key: "present", label: "Є", sortable: true },
        { key: "vacation", label: "V", sortable: true },
        { key: "doctorant", label: "А", sortable: true },
        { key: "sex", label: "С", sortable: true },
        { key: "birthYear", label: "Нар", sortable: true },
        { key: "inputYear", label: "Вст", sortable: true },
        { key: "graduationYear", label: "Вип", sortable: true },
        { key: "protectionYear", label: "Зах", sortable: true },
        { key: "p1", label: "1", sortable: true },
        { key: "p2", label: "2", sortable: true },
        { key: "p3", label: "3", sortable: true },
        { key: "p4", label: "4", sortable: true },
        { key: "p5", label: "5", sortable: true },
        { key: "p6", label: "6", sortable: true },
        { key: "p7", label: "7", sortable: true },
        { key: "info", label: "Інфо" },
        { key: "edit", label: "Зм" },
        { key: "delete", label: "Del" }
      ]
    };
  },

  mounted: function() {
    this.reloadList();
  },

  computed: {
    sortOptions() {
      // Create an options list from our fields
      return this.fields.filter(f => f.sortable).map(f => {
        return { text: f.label, value: f.key };
      });
    }
  },

  methods: {
    info(item, index, button) {
      this.modalInfo.title = `Row index: ${index}`;
      this.modalInfo.content = JSON.stringify(item, null, 2);
      this.$root.$emit("bv::show::modal", "modalInfo", button);
    },
    resetModal() {
      this.modalInfo.title = "";
      this.modalInfo.content = "";
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },

    // -------------- Загрузить данные с контроллера --------------
    reloadList() {
      axios.get(`api/Aspirants`).then(response => {
        this.list = response.data;
        //       this.items = this.list;
      });
    },

    // -------------- Добавить --------------
    addItem() {
      this.$router.push("/aspirant-add");
    },

    // -------------- Редактировать --------------
    editItem(item) {
      this.$router.push({
        name: "AspirantEdit",
        params: { id: item.id }
      });
    },

    removeItem(id) {
      axios.delete("api/Aspirants/" + id).then(response => {
        this.list = this.list.filter(item => {
          return item.id !== id;
        });
      });
    },

    // -------------- Импортировать из файла Excel --------------
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

          // { range: 1 } - читать начиная со 2 строки
          let Json = XLSX.utils.sheet_to_json(ws, { range: 1 });

          axios
            .post(`api/aspirants/import`, Json)
            .then(response => resolve("result"))
            .catch(err => {
              reject(new Error(err.response.data));
            });
        };
        reader.readAsArrayBuffer(file);
      });

      importPromise.then(
        result => this.reloadList(),
        error => {
          this.errorData = error.message;
          this.$refs.errorImportRef.show();
          this.reloadList();
        }
      );
    }
  }
};
</script>