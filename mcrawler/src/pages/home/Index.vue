<template>
  <!-- 登录框 -->
  <el-drawer
    v-model="loginDrawer"
    title="登录框"
    direction="ttb"
    :before-close="closeLogin"
    size="40%"
  >
    <el-form class="login-form" ref="loginRef" :model="loginInfo" status-icon>
      <el-form-item label="账号" prop="account">
        <el-input v-model="loginInfo.account" type="text" @keyup.enter="submitLogin" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="pwd">
        <el-input v-model="loginInfo.pwd" type="password" @keyup.enter="submitLogin" autocomplete="off"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitLogin">提交</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>

  <!-- 弹出框 -->
  <task-dialog
    ref="taskDialogRef"
    :form-type="dialogType"
    :task-info="selectedTaskInfo"
    @onSuccessed="onSuccessedFunc"
  ></task-dialog>

  <el-container style="height: 100%">
    <el-container>
      <!-- 左侧菜单 -->
      <task-menu v-if="config.showMenu"></task-menu>

      <el-container style="width: 100%">
        <!-- 头部操作 -->
        <el-header>
          <div class="header">
            <!-- <el-button icon="el-icon-search" size="medium" circle @click="login"></el-button> -->
            <el-button v-if="!isLogin" type="primary" @click="loginDrawer = true">登录</el-button>
            <el-avatar v-else="isLogin" :size="40" src="/empty.png" @error="errorHandler"></el-avatar>
          </div>

          <!-- 搜索框 -->
          <el-input
            v-model="searchText"
            @keyup.enter="search"
            placeholder="搜索关键字"
            class="input-with-select"
          >
            <template #append>
              <el-button icon="el-icon-search" @click="search"></el-button>
            </template>
          </el-input>
        </el-header>

        <!-- 主题内容 -->
        <el-main style="margin-top: 60px;text-align: center;">
          <!-- 任务主题 -->
          <div v-infinite-scroll="load" infinite-scroll-delay="1000" class="infinite-list">
            <el-row :gutter="15">
              <el-col
                :xs="24"
                :sm="12"
                :md="12"
                :lg="8"
                :xl="8"
                :span="8"
                v-for="(TaskInfo, index) in taskInfoList"
                :key="index"
              >
                <task-block
                  @onClick="onClickFunc(index)"
                  @onUpdate="onUpdateFunc(index)"
                  @onDelete="onDeleteFunc(index)"
                  :task-info="TaskInfo"
                  :img="TaskInfo.img"
                  class="task-block"
                ></task-block>
              </el-col>
              <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8" :span="8">
                <task-block-add
                  class="task-block"
                  @click="dialogType = 'create'; selectedTaskInfo = {}; taskDialogRef.open()"
                ></task-block-add>
              </el-col>
            </el-row>

            <!-- <el-row
              :gutter="15"
              :justify="space - around"
              v-for="(row, rowIndex) in rowNum"
              :key="rowIndex"
            >
              <el-col
                :xs="24"
                :sm="12"
                :md="12"
                :lg="8"
                :xl="8"
                :span="8"
                v-for="(colItem,colIndex) in colItems"
                :key="rowIndex * 3 + colIndex"
              >
                <TaskBlock
                  @click="electItem(rowIndex * 3 + colIndex)"
                  :task-info="colItem"
                  class="task-block"
                ></TaskBlock>
              </el-col>
            </el-row>-->
            <!-- <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8" :span="8">
              <task-block-add @click="electItem()" class="task-block"></task-block-add>
            </el-col>-->
          </div>

          <!-- <template>
            <el-pagination background layout="prev, pager, next" :total="1000"></el-pagination>
          </template>-->

          <!-- 任务主题end -->
        </el-main>
      </el-container>
    </el-container>
  </el-container>
</template>

<script>
// 导入组件
import TaskBlock from "../../components/TaskBlock.vue";
import TaskBlockAdd from "../../components/TaskBlockAdd.vue";
import TaskDialog from "../../components/TaskDialog.vue";
import TaskMenu from "../../components/TaskMenu.vue";
// 导入数据
import view_model from "../../view-model/home.js"
let { data, coms, mths } = view_model
// vue apis
import { onMounted, onBeforeUnmount, onBeforeMount, onUpdated, provide, toRefs } from 'vue'
import { getTaskList, setTaskList } from "../../util/help"

export default {
  components: {
    TaskBlock,
    TaskBlockAdd,
    TaskDialog,
    TaskMenu
  },
  setup() {

    /**
     * @description: 窗体关闭事件
     * @param {*}
     * @return {*}
     */
    window.onbeforeunload = function () {
      let list = data.taskInfoList
      if (list && list.length > 0) {
        let data = JSON.stringify(list)
        setTaskList(data)
      }
    };

    // 初始化
    onMounted(() => {
      let t = window.localStorage.getItem('token')
      if (t && t.length > 0) {
        data.isLogin = true;
      }


      data.taskInfoList.clear();
      if (data.isLogin) {
        mths.search();
      } else {
        let list = getTaskList();
        if (list && list.length > 0) {
          list = JSON.parse(list);
          list.forEach(item => data.taskInfoList.push(item));
        }
      }
      console.trace("onMounted")
    })

    onBeforeUnmount(() => {

      localStorage.setItem('write', 'unmount')

    })


    return {
      ...toRefs(data),
      ...mths,
      ...coms,
    }
  }
}

</script>

<style scoped>
.el-header,
.el-footer {
  background-color: #92b0f0;
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 56px;
}

.header {
  height: 60px;
  width: 100%;
  display: flex;
  align-items: center;
  flex-wrap: nowrap;
  justify-content: flex-end;
}

/* .el-aside {
  background-color: #d3dce6;
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 200px;
} */

.el-main {
  background-color: #d1f2f8;
  color: var(--el-text-color-primary);
  text-align: center;
}

.el-row {
  justify-content: flex-start;
  align-content: flex-start;
}

.infinite-list {
  height: 80vh;
  overflow-y: auto;
  overflow-x: hidden;
}

.login-form {
  margin: 0 auto;
}

@media only screen and (min-width: 768px) {
  .login-form {
    width: 50%;
  }
}

@media only screen and (min-width: 992px) {
  .login-form {
    width: 50%;
  }
}

@media only screen and (min-width: 1200px) {
  .login-form {
    width: 40%;
  }
}
</style>