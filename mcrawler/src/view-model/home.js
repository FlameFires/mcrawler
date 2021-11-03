import { reactive, ref, toRefs, computed } from 'vue'
import { account, task } from '../http/http.js'
import { ElLoading, ElMessage } from 'element-plus'

const data = reactive({
    taskDialogRef: null,
    config: {
        showMenu: false,
    },
    taskInfoList: [],
    selectedTaskInfo: {},
    searchText: '',
    page: 0,
    fullscreenLoading: false,
    tempRowIndex: 0,
    rowNum: 0,
    dialogType: 'create',
})

//#region computed
const colItems = computed({
    get: () => {
        const curIndex = data.tempRowIndex * 3;
        const colItems = data.taskInfoList.slice(curIndex, curIndex + 3)
        // ElMessage.info(data.tempRowIndex + ' - ' + JSON.stringify(colItems))
        return colItems
    },
})

//#endregion

//#region methods

function login() {
    account.login({ name: "15180582975", pwd: "123456" }).then(res => {
        if (res.ok) {
            ElMessage.success(JSON.stringify(res));
        } else {
            ElMessage.error(res.message);
        }
    });
}
/**
 * @description: 搜索
 * @param {*} event
 * @return {*}
 */
const search = () => {
    data.tempRowIndex = -1;
    const stext = data.searchText.trim();

    const loading = ElLoading.service({
        lock: true,
        text: 'Loading',
        spinner: 'el-icon-loading',
        background: 'rgba(0, 0, 0, 0.7)',
    })
    task.query({ key: stext }).then(res => {
        loading.close()

        if (res.ok) {
            data.taskInfoList = res.data;
        } else {
            ElMessage.error(res.message)
        }
    }).catch(() => {
        loading.close()
    });
}
/**
 * @description: 滑动到底部加载
 * @param {*}
 * @return {*}
 */
function load(p) {
    data.page += p !== undefined ? 0 : 1;
}
/**
 * @description: 转换图片路径
 * @param {*} index
 * @return {*}
 */
function imgpath(index) {
    let len = index % data.imgs.length;
    console.log(data.imgs[len]);
    return data.imgs[len];
}
/**
 * @description: 选中每个项目事件
 * @param {*} id
 * @return {*}
 */
const onClickFunc = index => {
    data.dialogType = "check"
    data.selectedTaskInfo = data.taskInfoList[index];
    data.taskDialogRef.open()
}

const onUpdateFunc = index => {
    data.dialogType = "update"
    data.selectedTaskInfo = data.taskInfoList[index];
    data.taskDialogRef.open()
}

const onSuccessedFunc = res => {
    if (res.ok) {
        search()
    }
}

//#endregion

export default {
    data,
    coms: {
        colItems,
    },
    mths: {
        login,
        search,
        load,
        imgpath,
        onClickFunc,
        onUpdateFunc,
        onSuccessedFunc
    },
}