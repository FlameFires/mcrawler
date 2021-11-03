export const useComponents = (array) => {
    let temp = {}
    array.forEach(name => import(`../components/${name}.vue`).then(module => { console.log(typeof (module), module, module.default) temp[name] = module.default }))
    // array.forEach(name => temp[toKebabCase(name)] = () => import(`../components/${name}.vue`))
    return temp
}

export const toKebabCase = str =>
    str && str
        .match(/[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+/g)
        .map(x => x.toLowerCase())
        .join('-')

export const getCurrentDate = (format = 2) => {
    var now = new Date()
    var year = now.getFullYear() //得到年份
    var month = now.getMonth()//得到月份
    var date = now.getDate()//得到日期
    var day = now.getDay()//得到周几
    var hour = now.getHours()//得到小时
    var minu = now.getMinutes()//得到分钟
    var sec = now.getSeconds()//得到秒
    month = month + 1
    if (month < 10) month = "0" + month
    if (date < 10) date = "0" + date
    if (hour < 10) hour = "0" + hour
    if (minu < 10) minu = "0" + minu
    if (sec < 10) sec = "0" + sec
    var time = ""
    //精确到天
    if (format == 1) {
        time = year + "-" + month + "-" + date
    }
    //精确到分
    else if (format == 2) {
        time = year + "-" + month + "-" + date + " " + hour + ":" + minu + ":" + sec
    }
    return time
}
