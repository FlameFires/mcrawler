import { defineComponent, computed, ref, toRefs, reactive, toRef, inject } from "vue"


const props = {
    img: {
        type: String,
        required: true,
        default: '../../static/imgs/1.jpg'
    },
    taskInfo: {
        type: Object,
        default: {
            taskIdaccountId: "814553d8-f755-4452-aba9-18b2a3a37f81",
            createDate: "2021/10/23 23:37:37",
            header: "string",
            method: "Delete",
            resolvePattern: "string",
            resolveType: "Regex",
            taskId: "35e51530-872d-4850-a41a-36b4897d4874",
            taskName: "string",
            taskParentId: "00000000-0000-0000-0000-000000000000",
            url: "string"
        }
    }
}

const data = reactive({
    img: props.img || "../../static/imgs/2.jpg"
})

const getCurrentDate = computed((format = 2) => {
    var now = new Date();
    var year = now.getFullYear(); //得到年份
    var month = now.getMonth();//得到月份
    var date = now.getDate();//得到日期
    var day = now.getDay();//得到周几
    var hour = now.getHours();//得到小时
    var minu = now.getMinutes();//得到分钟
    var sec = now.getSeconds();//得到秒
    month = month + 1;
    if (month < 10) month = "0" + month;
    if (date < 10) date = "0" + date;
    if (hour < 10) hour = "0" + hour;
    if (minu < 10) minu = "0" + minu;
    if (sec < 10) sec = "0" + sec;
    var time = "";
    //精确到天
    if (format == 1) {
        time = year + "-" + month + "-" + date;
    }
    //精确到分
    else if (format == 2) {
        time = year + "-" + month + "-" + date + " " + hour + ":" + minu + ":" + sec;
    }
    return time;
})

export default {
    data,
    props,
    mths: { getCurrentDate }
}