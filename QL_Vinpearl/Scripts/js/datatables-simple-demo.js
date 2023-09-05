window.addEventListener('DOMContentLoaded', event => {
    const datatablesSimple = document.getElementById('datatablesSimple');
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple, {
            perPage: 5,
            labels: {
                placeholder: "Tìm kiếm...",
                perPage: "dòng trên bảng",
                info: "Đang xem {start} đến {end} của {rows} dòng",
                noResults: "Không có kết quả nào phù hợp với từ khoá của bạn!",
            }
            });

    }
});