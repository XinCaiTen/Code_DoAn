$(document).ready(function () {
    $("#product_name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Products/GetProductSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.product_name, value: item.product_name };
                    }))
                }
            })
        },
        minLength: 0,
        messages: {
            noResults: "Không tìm thấy sản phẩm",
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        }, focus: function (event, ui) {
            $("#product_name").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#product_name").val(ui.item.label);
            return false;
        }
    })

})