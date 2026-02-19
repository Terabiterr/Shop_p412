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
                PasswordHash: '1234'
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
    return await fetch(
        url_products,
        {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        }
    ).then(response => {
        if(!response.ok)
            throw new Error('Fail to get products ...')
        return response.json()
    }).then(products => {
        let result = ''
        products.forEach(p => {
            result += 
            `
                <div class="card" style="width: 18rem;">
                    <img src="./img/product_test.jpg" class="card-img-top" alt="">
                    <div class="card-body">
                        <h5 id="titleId" class="card-title">${p.name}</h5>
                        <p id="descriptionId" class="card-text">${p.description}</p>
                        <p id="priceId" class="card-text">${p.price}</p>
                        <a href="#" class="btn btn-primary">buy</a>
                    </div>
                </div>
            `
        });
        document.getElementById("products")
        .innerHTML = result
    })
}
loadProducts()
