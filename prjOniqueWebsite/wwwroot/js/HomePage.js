function LoadCarousel() {
    imageCount = document.querySelectorAll(
        ".myCarousel .myContainer img"
    ).length

    const bottom = document.querySelector(".myCarousel .bottom")

    for (let i = 0; i < imageCount; i++) {
        const indicator = document.createElement("div")
        indicator.classList.add("indicator")
        indicator.onclick = () => setIndex(i)

        bottom.append(indicator)
    }
}

let imgIndex = 0

let imageCount = 0



function createAuto() {
    return setInterval(() => {
        imgIndex++
        refresh()
    }, 3000)
}

let autoTimer = createAuto()
function refresh() {
    if (imgIndex < 0) {
        imgIndex = imageCount - 1
    }
    else if (imgIndex > imageCount - 1) {
        imgIndex = 0
    }

    let myCarousel = document.querySelector('.myCarousel')

    let width = getComputedStyle(myCarousel).width

    width = Number(width.slice(0, -2))

    myCarousel.querySelector(".myContainer").style.left =
        imgIndex * width * -1 + "px"
}
let refreshWrapper = (func) => {
    return function (...args) {
        let result = func(...args)
        refresh()

        clearInterval(autoTimer)
        autoTimer = createAuto()
        return result
    }
}
let leftShift = refreshWrapper(() => {
    imgIndex--
})

let rightShift = refreshWrapper(() => {
    imgIndex++
})

let setIndex = refreshWrapper((idx) => {
    imgIndex = idx
})

refresh()