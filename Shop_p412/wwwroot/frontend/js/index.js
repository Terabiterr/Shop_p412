const url_server = `http://localhost:5286`
async function getToken() {
    const url_auth = `${url_server}/api/apiusers/login`
    return await fetch(
        url_auth,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Username: "admin",
                email: 'admin@gmail.com',
                PasswordHash: '12345'
            })
        }
    ).then(response => {
        if(!response.ok)
            throw new Error('Fail to fetch JWT Token ...')
        return response.json()
    }).then(data => {
        return data.token.result
    })
    .catch(err => console.log(err))
}
async function loadProducts() {
    const url_products = `${url_server}/api/apiproducts`
    const token = await getToken()

    return await fetch(url_products, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => {
        if (!response.ok)
            throw new Error('Fail to get products ...')
        return response.json()
    })
    .then(products => {
        console.log(products)

        products.forEach(p => {
            let div_product = document.createElement("div")
            div_product.setAttribute("class", "card_product")

            div_product.innerHTML = `<i>${p.name}</i><br>`

            let img_path = null

            if (p.productImages && p.productImages.length > 0) {
                img_path = p.productImages[0].imageUrl
            }

            if (img_path) {
                div_product.innerHTML += `
                    <img src="./img/${img_path}" alt="">
                `
            } else {
                div_product.innerHTML += `
                    <img src="./img/not_img.png" alt="">
                `
            }

            div_product.innerHTML += `
                <p>description: ${p.description}</p>
                <strong>price: ${p.price}</strong><hr>
                <strong>quantity: ${p.quantity}</strong><hr>
                <button class="btn_buy" onclick="add_to_cart(${p.id})">buy</button>
                `

            document.getElementById("products").appendChild(div_product)
        })
    })
}
/*
  Добавление в корзину
*/
async function add_to_cart(productId) {
    const token = await getToken()
    fetch(url_server + "/api/APICart/" + productId, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        }
    })
    .then(() => {
        //window.open('/cart');
    });
}
loadProducts()
