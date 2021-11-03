import axios from "./request"
//请求示例

const account = {
    login: (data) => {
        return axios({
            url: "Main/Account/Login",
            method: "post",
            data,
        })
    },
    register: (data) => {
        return axios({
            url: "Main/Account/Register",
            method: "post",
            data,
        })
    },
    aquire: (data) => {
        return axios({
            url: "Main/Account/Aquire",
            method: "get",
            data,
        })
    },
}

const task = {
    query: (data) => {
        return axios({
            url: `Main/Task/Query`,
            method: "get",
            data,
        })
    },
    add: (data) => {
        return axios({
            url: "Main/Task/Add",
            method: "post",
            data,
        })
    },
    aquire: data => {
        return axios({
            url: "Main/Task/Aquire",
            method: "get",
            data,
        })
    },
    update: data => {
        return axios({
            url: "Main/Task/Update",
            method: "put",
            data,
        })
    },
    start: data => {
        return axios({
            url: "Main/Task/Start",
            method: "get",
            data,
        })
    },
    delete: data => {
        return axios({
            url: "Main/Task/Delete",
            method: "delete",
            data,
        })
    },
    pause: data => {
        return axios({
            url: "Main/Task/Pause",
            method: "get",
            data,
        })
    },
}


export { account, task };

