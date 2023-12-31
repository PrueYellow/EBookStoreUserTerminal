<template>
  <div class="row">
    <div class="col-3"></div>
    <div class="col-6"></div>
    <div class="col-3"></div>
    <h2>書籍一覽</h2>
  </div>

  <!-- 全部書籍 -->

  <v-container>
    <el-row class="button-row">
      <el-col :span="1"> </el-col>
      <el-col :span="20">
        <div class="card-container">
          <el-col
            v-for="(book, index) in displayedBooks"
            :key="index"
            :span="columnCount"
            :xs="24"
            :sm="12"
            :md="8"
            :lg="6"
            :xl="6"
          >
            <el-card
              :body-style="{ padding: '0px' }"
              style="margin-right: 10px; margin-bottom: 10px"
            >
              <a :href="`/books/${book.id}`">
                <img
                  :src="`/src/BooksImage/${book.bookImage}`"
                  style="height: 300px; width: auto; max-width: 100%"
                />
              </a>
              <div style="padding: 20px; margin: 16px">
                <span class="book-title">{{ book.name }}</span>
                <a :href="`/booksearchauthor?searchString=${book.author}`">{{
                  book.author
                }}</a
                ><span> 著</span>
                <div
                  class="bottom"
                  style="
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                  "
                >
                  <span class="pricecolor" style="margin-top: 4px"
                    >{{ book.price }} 元</span
                  >
                  <BookCartbtn @add-to-cart="addToCart" :book="book" />
                </div>
              </div>
            </el-card>
          </el-col>
        </div>
        <!-- 分頁 -->
        <div class="demo-pagination-block container">
          <div class="demonstration">Change page size</div>
          <el-pagination
            v-model:current-page="currentPage2"
            v-model:page-size="pageSize2"
            :page-sizes="[12, 24, 36, 48]"
            :small="small"
            :disabled="disabled"
            :background="background"
            layout="sizes, prev, pager, next"
            :total="filteredBooks.length"
            @size-change="handleSizeChange"
            @current-change="handleCurrentChange"
          />
        </div>
      </el-col>
      <el-col :span="1"> </el-col>
    </el-row>
  </v-container>
</template>
    
  
  
  <script setup lang="ts">
import { ref, computed, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const books = ref([]);
const currentPage = ref(1);
const category = ref("");

const searchString = ref("");

const loadBooks = async () => {
  try {
    const response = await fetch("https://localhost:7261/api/Books");
    if (!response.ok) {
      throw new Error(`Network response was not ok: ${response.status}`);
    }
    const datas = await response.json();
    books.value = datas;

    // 搜尋傳入值
    if (searchString.value) {
      books.value = books.value.filter((book) => {
        const keyword = searchString.value.toLowerCase();
        return (
          book.name.toLowerCase().includes(keyword) ||
          book.isbn.toLowerCase().includes(keyword) ||
          book.categoryName.toLowerCase().includes(keyword) ||
          book.author.toLowerCase().includes(keyword)
        );
      });
    }
  } catch (error) {
    console.error("Error loading books:", error);
  }
};

onMounted(() => {
  searchString.value = route.query.searchString || "";
  loadBooks();
});

watch(route, () => {
  searchString.value = route.query.searchString || "";
  loadBooks();
  currentPage.value = 1;
});
//篩選分類書籍
const filteredBooks = computed(() => {
  if (!category.value) {
    return books.value;
  }
  return books.value.filter((book) => book.categoryName === category.value);
});

//分頁邏輯

const displayedBooks = computed(() => {
  const startIndex = (currentPage2.value - 1) * pageSize2.value;
  const endIndex = startIndex + pageSize2.value;
  return filteredBooks.value.slice(startIndex, endIndex);
});

const currentPage2 = ref(1);
const pageSize2 = ref(12);

const small = ref(false);
const background = ref(false);
const disabled = ref(false);
const handleSizeChange = (val: number) => {
  pageSize2.value = val;
  currentPage2.value = 1;
};
const handleCurrentChange = (val: number) => {
  currentPage2.value = val;
};
</script>
    
<script lang="ts">
import { defineComponent } from "vue";
import BooksNewDate from "./BookSearchFromNewsDate.vue";
import BookCartbtn from "./BookCartBtn.vue";

export default defineComponent({
  //新增
  data() {
    return {
      screenWidth: window.innerWidth, // 獲取視窗寬度
    };
  },
  //
  components: {
    BooksNewDate,
    BookCartbtn,
  },
  computed: {
    columnCount() {
      if (this.screenWidth < 768) {
        return 3; // 手機模式下顯示3本
      } else {
        return 8;
      }
    },
  },
  created() {
    window.addEventListener("resize", this.handleResize);
  },
  beforeUnmount() {
    window.removeEventListener("resize", this.handleResize);
  },
  methods: {
    handleResize() {
      this.screenWidth = window.innerWidth;
    },
  },
});
</script>
  



  <style src="../BookCSS/BookCSS.css" scoped>
.cuscard {
  margin: 10px;
}

.demo-pagination-block + .demo-pagination-block {
  margin-top: 10px;
}
.demo-pagination-block .demonstration {
  margin-bottom: 16px;
}
</style>
    