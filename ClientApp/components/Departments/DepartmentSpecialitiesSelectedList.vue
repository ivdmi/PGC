<template>
  <div>
    <vue-good-table
      ref="selected-table"
      :sort-options="{
            enabled: true,
            initialSortBy: [{field: 'id', type: 'asc'}]
            }"
      :select-options="{ 
              enabled: false,
    selectOnCheckboxOnly: true, // only select when checkbox is clicked instead of the row    
    selectionInfoClass: 'visibility-collapse',
    selectionText: '',
    clearSelectionText: '',          
          }"
      :columns="columns"
      :rows="list"
      row-style-class="font-14"
      styleClass="vgt-table condensed bordered"
    >
      <!-- <div slot="selected-row-actions" slot-scope="props">
    <button type="reset" class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Додати до наказу</button>
      </div>-->
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'Delete'">
          <button
            type="reset"
            class="btn btn-warning btnxs"
            @click="removeItem(props.row.id)"
            title="Видалити запис"
          >X</button>
        </span>
        <span v-else>{{props.formattedRow[props.column.field]}}</span>
      </template>
    </vue-good-table>
  </div>
</template>

<script>
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "DepartmentSpecialitiesSelectedList",
  components: {
    VueGoodTable
  },
  props: ["list"],

  data() {
    return {
      columns: [
        {
          label: "Код",
          field: "id",
          type: "number",
          width: "75px"
        },
        {
          label: "Спеціальність",
          field: "name",
          width: "500px"
        },
        {
          label: "Шифр",
          field: "knowledgeId",
          type: "number",
          width: "30px"
        },
        {
          label: "Галузь",
          field: "knowledge.name"
        },
        {
          label: "-",
          field: "Delete",
          width: "30px"
        }
      ]
    };
  },

  methods: {
    removeItem(param) {
      // отправить event del-speciality с параметром родителю
      this.$emit("del-speciality", param);
    }
  }
};
</script>