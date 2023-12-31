<template>
    <v-container class="d-flex flex-column mt-3" style="width: 900px">
        <img class="titleImage" :src="`/src/ArticleImage/${article.image}`" />
        <p class="text-h4">{{ article.title }}</p>
        <p class="text-subtitle-1">
            發表於 {{ article.createdTime }} By
            <a :href="`/articleList/${article.writerId}`">{{ article.writerName }}</a>
        </p>
        <div>
            <v-card>
                <v-tabs v-model="tab" bg-color="light">
                    <v-tab value="Profile">作者</v-tab>
                    <v-tab value="Articles">最新文章</v-tab>
                </v-tabs>

                <v-card-text>
                    <v-window v-model="tab">
                        <v-window-item value="Profile">
                            <v-row class="mt-1">
                                <v-col class="col-2 d-flex justify-content-center align-items-start">
                                    <img class="writerPhoto" :src="`/src/WriterImage/${article.writerPhoto}`" />
                                </v-col>
                                <v-col class="d-flex flex-column justify-content-center">
                                    <p class="text-h5 font-weight-bold">
                                        {{ article.writerName }}
                                    </p>
                                    <p class="text-h6">{{ article.writerProfile }}</p>
                                </v-col>
                            </v-row>
                        </v-window-item>

                        <v-window-item value="Articles">
                            <div>
                                <a class="text-h6" v-for="(item, index) in writer" :key="index"
                                    :href="`/article/${item.articleId}`">{{ index + 1 }}. {{ item.articleTitle }}
                                    <span class="text-black">
                                        - 發表於{{ item.createdTime.substring(0, 10) }}</span><br /></a>
                            </div>
                        </v-window-item>
                    </v-window>
                </v-card-text>
            </v-card>
        </div>
        <br />
        <p class="text-h6" v-for="paragraph in articleParagraphs">
            {{ paragraph }}
        </p>
        <div class="d-flex flex-row ml-4">
            <div class="d-flex align-items-center border pl-3 mr-3">
                <span>點此了解</span>
                <v-icon right icon="mdi:mdi-arrow-right-thick" />
            </div>
            <v-btn size="large" class="bookbtn" :href="`/books/${article.bookId}`">
                {{ article.bookName }}</v-btn>
        </div>
    </v-container>
</template>
      
<script setup >
import { ref, onMounted, watch } from "vue";
import { useRoute } from "vue-router";

const tab = ref(null);
const article = ref([]);
const articleId = ref("");
const articleParagraphs = ref([]);
const writer = ref([]);

// 取得Article ID
const route = useRoute();
watch(
    () => {
        articleId.value = route.params.articleid;
    },
    { immediate: true }
);

//取得WebAPI資料
const loadArticle = async () => {
    try {
        const response = await fetch(
            `https://localhost:7261/api/Articles/${articleId.value}`
        );
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const datas = await response.json();
        article.value = datas[0];
        article.value.createdTime = article.value.createdTime.substring(0, 10);
        article.value.bookName = article.value.bookName.substring(0, 20);
        articleParagraphs.value = article.value.content.trim().split(/\r\n\r\n/);
    } catch (error) {
        console.error("Error loading books:", error);
    }
};
const loadWriter = async () => {
    try {
        const response = await fetch(
            `https://localhost:7261/api/Articles/writer/${article.value.writerId}`
        );
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.status}`);
        }
        const datas = await response.json();
        writer.value = datas;
        // console.log(writer.value);
    } catch (error) {
        console.error("Error loading books:", error);
    }
};

onMounted(async () => {
    await loadArticle();
    loadWriter();
});
</script>
      
<style>
.titleImage {
    margin-bottom: 30px;
    height: 400px;
    width: auto;
    max-width: 100%;
}

.writerPhoto {
    border-radius: 40px;
    width: 80px;
    height: 80px;
}

.bookbtn {
    border-radius: 40px;
    font-size: 30px;
    background-color: #ffff8d;
}
</style>