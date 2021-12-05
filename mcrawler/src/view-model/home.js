import { reactive, ref, toRefs, computed } from 'vue'
import { account, task } from '../http/http.js'
import { ElLoading, ElMessage, ElMessageBox } from 'element-plus'

const data = reactive({
    config: {
        showMenu: false,
    },
    // 是否登录
    isLogin: false,
    loginDrawer: false,
    loginRef: null,
    loginInfo: {
        account: '',
        pwd: '',
    },
    // 任务详情框引用
    taskDialogRef: null,
    // 任务列表
    taskInfoList: [],
    // 选中的任务
    selectedTaskInfo: {},
    // 搜索文本
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
    console.trace({ page, p }, 'scroll')
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
    console.trace()
}

const onDeleteFunc = function (index) {
    ElMessageBox.confirm(
        '你确定删除吗?',
        '警告',
        {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning',
        }
    ).then(() => {
        let taskInfo = data.taskInfoList[index];
        if (data.isLogin) {
            task.delete({ id: taskInfo.taskId })
                .then(function (res) {
                    if (res.ok) {
                        data.taskInfoList.del(index)
                        ElMessage.success("删除成功！")
                    } else {
                        ElMessage.error(res.msg)
                    }
                })
        } else {
            data.taskInfoList.del(index)
        }
    })
}

const onSuccessedFunc = res => {
    if (res.ok) {
        search()
    }
}

/**
 * @description: 关闭登录窗口
 * @param {*}
 * @return {*}
 */
const closeLogin = () => {
    data.loginDrawer = false
    data.loginInfo.account = ''
    data.loginInfo.pwd = ''
}



/**
 * @description: 登录提交
 * @param {*} formName
 * @return {*}
 */
const submitLogin = function (formName) {
    data.loginRef.validate((valid) => {
        if (valid) {
            // account.login({ name: "15180582975", pwd: "123456" })
            account.login({
                name: data.loginInfo.account,
                pwd: data.loginInfo.pwd
            })
                .then(res => {
                    if (res.ok) {
                        // ElMessage.success(JSON.stringify(res));
                        ElMessage.success("登录成功");
                        data.isLogin = true;
                        setTimeout(() => {
                            data.loginDrawer = false
                        }, 1000);
                    } else {
                        ElMessage.error(res.msg);
                    }
                });
        } else {
            console.log('error submit!!')
            return false
        }
    })
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
        onSuccessedFunc,
        onDeleteFunc,
        closeLogin,
        submitLogin,
    },
}