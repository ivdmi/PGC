<template>
  <div>
    <div>
      <div>
        <vue-good-table
        ref="specialities-table"
          :select-options="{ 
            enabled: true,
            selectOnCheckboxOnly: true     
          }"
          :columns="columns"
          :rows="list"          
          :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 300,
            perPageDropdown: [300, 20, 50],
            setCurrentPage: 1,
            nextLabel: '',
            prevLabel: '',
            rowsPerPageLabel: 'Строк',
            ofLabel: 'з',
            pageLabel: 'Стор.', // for 'pages' mode
            allLabel: 'All',
            position: 'top'
          }"
          :search-options="{
            enabled: true
          }"
          row-style-class="font-14"
          styleClass="vgt-table condensed bordered"
          >
  <div slot="selected-row-actions" slot-scope="props">
    <button type="reset" class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Додати обрані спеціальності</button>
  </div>

  <template slot="table-row" slot-scope="props">
    <span v-if="props.column.field == 'Add'">      
      <button type="reset" class="btn btn-warning btnxs" @click="addItem(props.row)" title="Додати запис">+</button>
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
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "DepartmentSpecialitiesList",
  components: {
    VueGoodTable,
  },

  data() {
    return {
      list: [],  
      columns: [
        {
          label: "Код",
          field: "id",
          type: "number",
          width: "35px",
        },        
        {
          label: "Спеціальність",
          field: "name",
          width: "500px",
        },
        {
          label: "Шифр",
          field: "knowledgeId",
          type: "number",
          width: "30px",
        },
        {
          label: "Галузь",
          field: "knowledge.name",
        },
        {
          label: "-",
          field: "Add",
          width: "30px",
        }
      ],
    };
  },

  mounted: function() {
    axios
      .get(`api/specialities`).then(response => {
        this.list = response.data;
      })
  },

  methods: {  
    selectionAction(params) {
      // скопировать отмеченные строки, вручную снять отметки (type="reset" - отменяет Submit для кнопки "Додати спеціальність")
      var specialities = this.$refs["specialities-table"].selectedRows.slice();
      this.$refs["specialities-table"].unselectAllInternal();
      // отправить event add-aspirants с параметром родителю
      this.$emit('add-specialities', specialities)
    },
    addItem(param) {
      // отправить event add-speciality с параметром родителю
      this.$emit('add-speciality', param)
    },
  }
};
</script>