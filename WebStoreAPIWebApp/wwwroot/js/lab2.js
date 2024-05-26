const uri = 'api/Carts';
let carts = [];

function getCarts() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayCarts(data))
        .catch(error => console.error('Unable to get carts.', error));
}

function addCart() {
    const addCustomerIdTextbox = document.getElementById('add-customer-id');
    const addDeliveryAddressTextbox = document.getElementById('add-delivery-address');

    const cart = {
        customerId: addCustomerIdTextbox.value.trim(),
        deliveryAddress: addDeliveryAddressTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(cart)
    })
        .then(response => response.json())
        .then(() => {
            getCarts();
            addCustomerIdTextbox.value = '';
            addDeliveryAddressTextbox.value = '';
        })
        .catch(error => console.error('Unable to add cart.', error));
}

function deleteCart(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getCarts())
        .catch(error => console.error('Unable to delete cart.', error));
}

function displayEditForm(id) {
    const cart = carts.find(cart => cart.id === id);

    document.getElementById('edit-id').value = cart.id;
    document.getElementById('edit-customer-id').value = cart.customerId;
    document.getElementById('edit-delivery-address').value = cart.deliveryAddress;
    document.getElementById('editCart').style.display = 'block';
}

function updateCart() {
    const cartId = document.getElementById('edit-id').value;
    const cart = {
        id: parseInt(cartId, 10),
        customerId: document.getElementById('edit-customer-id').value.trim(),
        deliveryAddress: document.getElementById('edit-delivery-address').value.trim()
    };

    fetch(`${uri}/${cartId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(cart)
    })
        .then(() => getCarts())
        .catch(error => console.error('Unable to update cart.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editCart').style.display = 'none';
}

function _displayCarts(data) {
    const tBody = document.getElementById('carts');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(cart => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${cart.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteCart(${cart.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeCustomerId = document.createTextNode(cart.customerId);
        td1.appendChild(textNodeCustomerId);

        let td2 = tr.insertCell(1);
        let textNodeAddress = document.createTextNode(cart.deliveryAddress);
        td2.appendChild(textNodeAddress);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    carts = data;
}
