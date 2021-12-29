
    $(function () {
        var dateFormat = "dd/mm/yy",
            from = $("#birthday")
                .datepicker({
                    defaultDate: "+1w",
                    numberOfMonths: 1,
                    dateFormat: 'dd/mm/yy'
                });
    })