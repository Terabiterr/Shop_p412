// import { loadCart } from './index';
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
async function loadCart() {
    const token = await getToken()
    console.log(token)
    fetch(`${url_server}/api/apicart`, {
        headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
    })
    .then(res => res.json())
    .then(cart => {

        const container = document.getElementById("cart_products");
        container.innerHTML = "";

        if (!cart || !cart.items || cart.items.length === 0) {
            container.innerHTML = "Cart is empty";
            return;
        }

        let total = 0;

        cart.items.forEach(item => {
            total += item.price * item.quantity;

            container.innerHTML += `
                <div>
                    ${item.name}
                    ${item.quantity} x ${item.price}
                    <button onclick="removeItem(${item.productId})">
                        Remove
                    </button>
                </div>
            `;
        });

        container.innerHTML += "<h3>Total: " + total + "</h3>";
    });
}
loadCart()