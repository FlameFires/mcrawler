import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import router from './router'


/**
 * 插件集
 * @type {import("vue").Plugin[]}
 */
const plugins = [ElementPlus, router];


/**
 * @description: 工具函数注册
 * @param {*}
 * @return {*}
 */
(function () {
    // 数组清空
    Array.prototype.clear = function () {
        this.splice(0, this.length);
    }

    Array.prototype.del = function (index) {
        if (index >= 0 && this.length > 0) {
            this.splice(index, 1)
        }
    }

})();


/**
 * @description: 批量注册插件
 * @param {import("vue").App} app
 * @return {*}
 */
export const usePlugins = app => plugins.forEach(app.use, app)