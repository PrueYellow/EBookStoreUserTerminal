<template>
  <v-container class="py-0 fill-height">
    <v-row no-gutters>
      <v-col cols="3">
        <v-btn class="fill-height" flat color="white" router :to="homeRoute">
          <v-img class="" :width="60" aspect-ratio="4/3"
            src="https://blog.flamingtext.com/blog/2023/08/08/flamingtext_com_1691517616_586896794.png"></v-img>
          <span class="text-subtitle-1 text-grey-darken-3" style="display: flex; vertical-align: text-bottom">網路書店</span>
        </v-btn>
      </v-col>
      <v-col class="d-flex justify-center" cols="6">
        <v-responsive max-height="100" max-width="600">
          <el-autocomplete style="width: 400px" v-model="searchInput" :fetch-suggestions="querySearchAsync"
            placeholder="搜尋" @keydown.enter="goToSearchPage" @select="handleAutocompleteSelect">
            <template #append>
              <div class="append-container" @click="goToSearchPage">
                <i class="fas fa-search search-icon"></i>
                <div class="overlay"></div>
              </div>
            </template>
          </el-autocomplete>
        </v-responsive>
      </v-col>


      <v-col class="d-flex justify-end" cols="3">
        <v-btn @click="hiUserName">
          <a class="greeting" v-if="isLoggedIn" style="color: gray;">Hi, {{ account }}</a>
        </v-btn>
        <v-btn flat color="grey" router v-if="isLoggedIn" :to="cartRoute">
          <v-icon right icon="mdi:mdi-cart" />
          <v-badge floating color="error" :content="cartStore.cartItemsCount" overlap>
          </v-badge>
        </v-btn>
        <v-menu open-on-hover>
          <template v-slot:activator="{ props }">
            <v-btn color="grey" v-bind="props" v-if="isLoggedIn">
              <!-- <v-icon right icon="mdi:mdi-account" /> -->
                <UserPicture />
            </v-btn>
          </template>
          <v-list>
            <v-list-item v-for="(useritem, index) in useritems" :key="index" router :to="useritem.route">
              <v-list-item-title>{{ useritem.title }}</v-list-item-title>
            </v-list-item>
          </v-list>
        </v-menu>
        <!-- <v-btn flat color="grey" @click="logout" v-if="isLoggedIn || !$route.path.includes('/Login')"> -->
        <v-btn flat @click="logout" v-if="isLoggedIn && !$route.path.includes('/Login')">
          <a href="/Login" style="color: gray;">登出</a>
        </v-btn>
        <!-- <v-btn
          flat
          color="grey"
          @click="logout"
          v-if="!$route.path.includes('/Login')"
        >
          <v-icon right icon="mdi:mdi-exit-to-app" />
        </v-btn> -->
        <v-btn flat style="color: gray;" @click="logout" v-if="!isLoggedIn && !$route.path.includes('/Login')">
          <a href="/Login" style="color: gray;">登入</a>
        </v-btn>

        <!--<div>
            <v-btn flat style="color: gray;" @click="logoutButton">
            {{ isLoggedIn ? '登出' : '登入' }}
            </v-btn>
        </div>-->
      </v-col>

      <!-- <v-spacer></v-spacer> -->
    </v-row>
  </v-container>
</template>






<script setup lang="ts">
import { useCartStore } from '../stores/cart';

import { useRouter, useRoute } from "vue-router";
import axios from "axios";
import { ref, onMounted, watch } from "vue";
import { ElAutocomplete } from "element-plus";
import { googleLogout } from 'vue3-google-login';
import UserPicture from "../components/UserPicture.vue";


const cartStore = useCartStore();

const useritems = [
  { title: "會員中心", route: "/UserProfile" },
  { title: "歷史訂單", route: "/orders" },
];


const cartRoute = "/cart";
const homeRoute = "/";
const isLoggedIn = ref(false);
const searchInput = ref("");
const route = useRoute();

const account = ref("");

const hiUserName = () => {
    router.push("/UserProfile");
}

const logoutButton = () => {
  if (isLoggedIn.value) {
    logout();
    googleLogout();
    router.push('/Login');
  } else {
    router.push('/Login');
  }
};

const logout = () => {
  const userInfo = JSON.parse(localStorage.getItem("userInfo") || '{}');
  if (userInfo && userInfo.id) {
    localStorage.removeItem("userInfo");
    router.push("/Login");
  } else {
    router.push("/Login");
  }
};

watch(route, () => {
  const userInfo = JSON.parse(localStorage.getItem("userInfo") || '{}');
  if ((userInfo && userInfo.id) != null) {
    isLoggedIn.value = true;
    account.value = userInfo.account;
  } else {
    isLoggedIn.value = false;
    account.value = "";
  }
  // console.log(this.isLoggedIn)
});

// //查詢邏輯

const state = ref("");

interface LinkItem {
  value: string;
  categoryName: string;
  author: string;
}

const links = ref<LinkItem[]>([]);

async function loadAll(queryString: string) {
  if (queryString) {
    try {
      const apiUrl = "https://localhost:7261/api/Books/filter";
      const bookDto = {
        Name: queryString,
        CategoryName: queryString,
        Author: queryString,
        Id: 0,
      };

      const response = await axios.post(apiUrl, bookDto);
      console.log("Response Data:", response.data); //測試
      if (response.status === 200) {
        links.value = response.data.map((result: any) => {
          return {
            value: result.name,
            categoryName: result.categoryName,
            author: result.author,
          };
        });
      }
    } catch (error) {
      console.error("Error fetching suggestions from backend:", error);
    }
  }
}

let timeout: NodeJS.Timeout;
const querySearchAsync = (
  queryString: string,
  cb: (suggestions: any[]) => void
) => {
  loadAll(queryString); // 獲取後端查詢結果

  console.log("Links:", links.value); //測試

  clearTimeout(timeout);
  timeout = setTimeout(() => {
    cb(links.value);
  }, 3000 * Math.random());
};

const handleSelect = (item: LinkItem) => {
  searchInput.value = item.value;
  goToSearchPage();
};

const handleAutocompleteSelect = (item: Record<string, any>) => {
  const linkItem: LinkItem = {
    value: item.value,
    categoryName: item.categoryName,
    author: item.author,
  };
  handleSelect(linkItem);
};

onMounted(() => {
  loadAll("");

  cartStore.updateCartItemsCount(0);
  // getLogStatus();
});

const router = useRouter();
function goToSearchPage() {
  router.push({
    name: "book-searchall",
    query: { searchString: searchInput.value },
  });
}
</script>




<style>
.append-container {
  cursor: pointer;
}

.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: transparent;
  z-index: 1;
}

.greeting {
  color: gray;
  text-transform: none;
  /* 不會轉換成大寫 */
}

</style>
