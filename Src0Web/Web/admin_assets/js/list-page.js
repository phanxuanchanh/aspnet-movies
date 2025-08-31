function loadPage(e) {
    e.preventDefault();
    const pageSize = document.getElementById("pageSize").value;
    const keyword = document.getElementById("searchBox").value;
    location.href = `${url}&pageSize=${pageSize}&searchText=${keyword}`;
}

document.getElementById("searchBox").addEventListener("keydown", function (e) {
    if (e.key === "Enter") {
        loadPage(e);
    }
});

document.getElementById("pageSize").addEventListener("keydown", function (e) {
    if (e.key === "Enter") {
        loadPage(e);
    }
});


document.getElementById('selectAll').addEventListener('change', function (e) {
    const checkboxes = document.querySelectorAll('input[name="selected_ids[]"]');
    checkboxes.forEach(cb => cb.checked = e.target.checked);
});


document.getElementById('btnDeleteSelected').addEventListener('click', () => {
    const ids = Array.from(document.querySelectorAll('input[name="selected_ids[]"]:checked'))
        .map(cb => cb.value);

    if (ids.length === 0)
        return alert('Chọn ít nhất 1 item');

    const argument = ids.join(',');
    __doPostBack('btnDeleteSelected_Click', argument);
});