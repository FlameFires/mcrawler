// http/request.js
import instance from "./index"
import { ElMessage } from 'element-plus'
/**
 * @param {String} method  请求的方法：get、post、delete、put
 * @param {String} url     请求的url:
 * @param {Object} data    请求的参数
 * @param {Object} config  请求的配置
 * @returns {Promise}     返回一个promise对象，其实就相当于axios请求数据的返回值
 */

const errorHandler = error => {
    // let isDevelopment = process.env.NODE_ENV === "development";
    // if (error.response) {
    //     // The request was made and the server responded with a status code
    //     // that falls out of the range of 2xx
    //     console.log(error.response.data);
    //     console.log(error.response.status);
    //     console.log(error.response.headers);
    //     if (isDevelopment) ElMessage.error(JSON.stringify(error.response));
    // } else if (error.request) {
    //     // The request was made but no response was received
    //     // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
    //     // http.ClientRequest in node.js
    //     console.log(error.request);
    //     if (isDevelopment) ElMessage.error(JSON.stringify(error.request));
    // } else {
    //     // Something happened in setting up the request that triggered an Error
    //     console.log('Error', error.message);
    //     if (isDevelopment) ElMessage.error(JSON.stringify(error.message));
    // }
};

const axios = ({
    method,
    url,
    data,
    config
}) => {
    method = method.toLowerCase();
    if (method == 'post') {
        return instance.post(url, data, { ...config }).catch(errorHandler)
    } else if (method == 'get') {
        return instance.get(url, {
            params: data,
            ...config
        }).catch(errorHandler)
    } else if (method == 'delete') {
        return instance.delete(url, {
            params: data,
            ...config
        }).catch(errorHandler)
    } else if (method == 'put') {
        return instance.put(url, data, { ...config }).catch(errorHandler)
    } else {
        console.error('未知的method' + method)
        return false
    }
}
export default axios

