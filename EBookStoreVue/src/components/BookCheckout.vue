<template>
  <el-button class="button" round style="background-color: #f50d0d; color: #ebeff4" @click="addToCart"
    :disabled="props.book.stock === 0"><i class="fa-solid fa-cart-shopping fa-xl"
      style="color: #ebeff4; margin-right: 10px"></i>
    直接結帳</el-button>
</template>
  
  
<script setup>
import "@fortawesome/fontawesome-free/css/all.min.css";
import { ref, nextTick, defineExpose, defineProps } from "vue";
import { useRouter } from "vue-router";
import { toast } from "vue3-toastify";
import "vue3-toastify/dist/index.css";
import axios from "axios";
import { useCartStore } from '../stores/cart';

const cartStore = useCartStore();

const props = defineProps(["book"]);
//驗證登入

const userInfo = JSON.parse(localStorage.getItem("userInfo"));
const isLoggedIn = ref(userInfo && userInfo.id !== undefined);
const router = useRouter();

//點擊事件
// props.addToCart(book);
const addToCart = async () => {
  console.log(props.book); //抓取資料如果要抓書本名 console.log(props.book.name)
  //   props.book.stock
  //   props.book.id
  //   props.book.stock
  //   userInfo.id

  // 判斷庫存
  if (props.book.stock === 0) {
    toast("沒庫存", {
      autoClose: 1000,
      position: "bottom-right",
    });
    return;
  }

  // 購物車操作
  if (isLoggedIn.value) {
    const Url = "https://localhost:7261/CartsButton";
    const CartDto = {
      UserId: userInfo.id,
      BookId: props.book.id,
      Qty: 1,
      Id: 0,
      payment: 0,
    };
    const response = await axios.post(Url, CartDto);
    toast(response.data.message, {
      autoClose: 1000,
      position: 'bottom-right',
    });
    cartStore.updateCartItemsCount();
    router.push("/cart");
  } else {
    nextTick(() => {
      router.push("/Login");
    });
  }
};

defineExpose({
  addToCart,
});
</script>
  
  
<style></style>