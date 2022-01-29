var param

window.onload = function () {
    
    let tmp = window.location.search.replace('?', '')
    console.log(tmp)
}

function createCard(bike) {
    var body = document.createElement("div")
    body.classList.add("card", "m-3", "border", "border-info", "border-2", "rounded-3")
    body.style = "width:20rem;"

    var img = document.createElement("img")
    img.src = bike.imgUrl
    img.className = "card-img-top"
    img.style = "height:40%;"

    var title = document.createElement('div')
    title.className = "card-body"
    var h = document.createElement('h5')
    h.innerText=bike.model
    title.append(h)

    var list = document.createElement('ul')
    list.classList.add("list-group","list-group-flush")
    var li1 = document.createElement("li")
    var li2 = document.createElement("li")
    var li3 = document.createElement("li")
    var li4 = document.createElement("li")
    li1.className = "list-group-item"
    li2.className = "list-group-item"
    li3.className = "list-group-item"
    li4.className = "list-group-item"
    li1.innerText = `Country: ${bike.country}`
    li2.innerText = `Color: ${bike.color}`
    li3.innerText = `Weight: ${bike.weight}`
    li4.innerText = `Price: ${bike.price}`
    list.append(li1)
    list.append(li2)
    list.append(li3)
    list.append(li4)

    var lowCont = document.createElement('div')
    lowCont.classList.add("card-body", "d-flex", "justify-content-between")

    var edit = document.createElement('a')
    edit.classList.add("btn", "btn-warning")
    edit.style = "width:30%;height:fit-content"
    edit.innerText = "Edit"
   /* edit.value=bike.bicycleId*/
    edit.href =`create.html/?id=${bike.bicycleId}`

    var delForm = document.createElement('form')
    delForm.method = "delete"
    var hidden = document.createElement('input')
    hidden.type = "hidden"
    hidden.name = "id"
    hidden.value = bike.bicycleId
    var btn = document.createElement('input')
    btn.value = "Delete"
    btn.type = "submit"
    btn.classList.add("btn", "btn-danger")
    btn.addEventListener('click', async function(e) {
        e.preventDefault()
        await fetch(`api/bicycles/${bike.bicycleId}`, {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json',
                 'Authorization': 'bearer ' + sessionStorage.getItem('access_token')
            }
            
        })
         refresh()
    })
    delForm.append(hidden)
    delForm.append(btn)

    lowCont.append(edit)
    lowCont.append(delForm)

    body.append(img)
    body.append(title)
    body.append(list)
    body.append(lowCont)

    return body
}

function update(e) {
    /*window.location.href = 'create.html'*/
    
    window.onblur = function () {
        let id = this.value
        fetch(`api/bicycles/${id}`).then(y => y.json()).then(x => {
            const form = document.forms['bikeForm']
            form.elements['Model'].value = x.model
            form.elements['Country'].value = x.country
            form.elements['Color'].value = x.colore
            form.elements['Weight'].value = x.weight
            form.elements['Price'].value = x.price
            form.elements['ImgUrl'].value = x.imgUrl
            form.elements['bicycleId'].value = x.bicycleId
        })
    }

}

async function refresh() {
    let main = document.getElementById('cont')
    while (main.firstChild) {
       await main.removeChild(main.firstChild);
    }
    await getList()
}

async function getList() {
    const response = await fetch('api/bicycles', {
        headers: {
            'Authorization': 'bearer ' + sessionStorage.getItem('access_token')
        }
    })
    if (response.ok === true) {
        const bikes = await response.json()
        let main = document.getElementById('cont')
        bikes.forEach(card=>main.append(createCard(card)))
    }
}


async function createBike(model, country, color, weight, price, imgUrl) {
    const response = await fetch('api/bicycles', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + sessionStorage.getItem('access_token')
        },
        body: JSON.stringify({
            model,
            color,
            country,
            weight: parseFloat(weight),
            price: +price,
            imgUrl
        })
    })
    if (response.ok === true) {
        window.location.href = 'index.html'
        var er=document.getElementById('errors').classList.add('d-none')
        while (er.firstChild) {
            er.removeChild(er.firstChild);
        }
    } else {
        //const errorData = await response.json()
        //console.log(errorData)
        //console.log(errorData.errors)
        //if (errorData) {
        //    if (errorData.errors['Model']) {
        //        showError(errorData.errors['Model'])
        //    }
        //    if (errorData.errors['Country']) {
        //        showError(errorData.errors['Country'])
        //    }
        //    if (errorData.errors['Color']) {
        //        showError(errorData.errors['Color'])
        //    }
        //    if (errorData.errors['Weight']) {
        //        showError(errorData.errors['Weight'])
        //    }
        //    if (errorData.errors['Price']) {
        //        showError(errorData.errors['Price'])
        //    }
        //    if (errorData.errors['ImgUrl']) {
        //        showError(errorData.errors['ImgUrl'])
        //    }
        //}
        document.getElementById('errors').classList.remove('d-none')
        document.getElementById('errors').innerHTML="<b>All fields must be filled</b>"
    }
}
function showError(errors) {
    errors.forEach(er => {
        const p = document.createElement('p')
        p.append(er)
        document.getElementById('errors').append(p)
    })
}
if (window.location.href == 'https://localhost:44304/create.html') {
    document.forms['bikeForm'].addEventListener('submit', function (e) {
        e.preventDefault()
        const form = document.forms['bikeForm']
        var model = form.elements['Model'].value
        var country = form.elements['Country'].value
        var color = form.elements['Color'].value
        var weight = form.elements['Weight'].value
        var price = form.elements['Price'].value
        var img = form.elements['ImgUrl'].value
        if (form.elements['bicycleId'].value == 0) {
            createBike(model, country, color, weight, price, img)
        }
        else {
            editBike(form.elements['bicycleId'].value, model, country, color, weight, price, img)
        }
    })
}
else {
    getList()
    document.getElementById('submitButton').addEventListener('click', async function () {
        await getTokenAsync()
        getList()
    })

}



async function editBike(bicycleId,model, country, color, weight, price, imgUrl) {
    const response = await fetch('api/bicycles', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
             'Authorization': 'bearer ' + sessionStorage.getItem('access_token')
        },
        body: JSON.stringify({
            bicycleId: +bicycleId,
            model,
            color,
            country,
            weight: parseFloat(weight),
            price: +price,
            imgUrl
        })
    })
    if (response.ok === true) {
        window.location.href = 'index.html'
        var er = document.getElementById('errors').classList.add('d-none')
        while (er.firstChild) {
            er.removeChild(er.firstChild);
        }
    } else {
        document.getElementById('errors').classList.remove('d-none')
        document.getElementById('errors').innerHTML = "<b>All fields must be filled</b>"
    }
}

async function getTokenAsync() {
    const credentials = {
        login: document.querySelector('#login').value,
        password: document.querySelector('#password').value
    }

    const response = await fetch('api/account/token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(credentials)
    })

    const data = await response.json()
    var tmp = document.getElementById('failLog')
    if (response.ok === true) {
        sessionStorage.setItem('access_token', data.access_token)
        tmp.classList.add('d-none')
        tmp.innerText=""
    } else {
        tmp.classList.remove('d-none')
        tmp.innerText =`Error ${response.status}, ${data.error} !`
        console.log(response.status,data.error)
    }
}

