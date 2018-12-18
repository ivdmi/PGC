<template>
    <div>
        <BaseInputText 
            v-model="newTodoText"
            placeholder="New todo item"
            @keydown.enter="addTodo"
        />
        <ul v-if="todos.length">
            <TodoListItem
                v-for="todo in todos"
                :key="todo.id"
                :todo="todo"
                @remove="removeTodo"
            />
        </ul>

        
        <p v-else>
            Nothing left in the list. Add a new todo in the input above.
        </p>
        <b-table v-bind:items="items"></b-table>
    </div>
</template>

<script>
import BaseInputText from "./BaseInputText.vue";
import TodoListItem from "./TodoListItem.vue";

let nextTodoId = 1;

export default {
  components: {
    BaseInputText,
    TodoListItem
  },
  data() {
    return {
      newTodoText: "",
      todos: [
        {
          id: nextTodoId++,
          text: "Learn Vue"
        },
        {
          id: nextTodoId++,
          text: "Learn about single-file components"
        },
        {
          id: nextTodoId++,
          text: "Fall in love"
        }
      ],

      items: [
        { first: "Dickerson", last: "Macdonald" },
        { first: "Larsen", last: "Shaw" },
        { first: "Geneva", last: "Wilson" },
        { first: "Jami", last: "Carney" }
      ]
    };
  },
  methods: {
    addTodo() {
      const trimmedText = this.newTodoText.trim();
      if (trimmedText) {
        this.todos.push({
          id: nextTodoId++,
          text: trimmedText
        });
        this.newTodoText = "";
      }
    },
    removeTodo(idToRemove) {
      this.todos = this.todos.filter(todo => {
        return todo.id !== idToRemove;
      });
    }
  }
};
</script>