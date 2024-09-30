document.addEventListener('DOMContentLoaded', function () {
    getUsers()
})

function getUsers() {
    const url = `https://localhost:7297/api/home`
    fetch(url)
        .then(res => res.json())
        .then(data => {
            console.log(data)
            let tbody = document.getElementById('users_tbody_id')
            let row = ''
            data.forEach(u => {
                row += '<tr>'
                row += `<td>${u.id}</td>`
                row += `<td>${u.firstName}</td>`
                row += `<td>${u.lastName}</td>`
                row += `<td>${u.email}</td>`
                row += '</tr>'
                tbody.innerHTML += row
            })
        })
}