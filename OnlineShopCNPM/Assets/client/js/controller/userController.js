var user = {
    init: function () {
        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != "") {
                idProvince = Number(id);
                user.loadDistrict(idProvince);
            }
            else {
                idProvince = -1;
                $('#ddlDistrict').html('');
            }
        })
        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != "") {
                idDistrict = Number(id);
                user.loadSubDistrict(idProvince, idDistrict);
            }
            else {
                idDistrict = -1;
                $('#ddlSubDistrict').html('');
            }
        })
    },
    loadProvince: function () {
        $.ajax({
            url: '/User/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = "<option value = '' style = 'display : none;'>--Chọn tỉnh/thành--</option>";
                    $.each(data, function (i, item){
                        html += "<option value = '" + item.ID + "'>" + item.Name + "</option>";
                    });
                    $("#ddlProvince").html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/User/LoadDistrict',
            type: "POST",
            data: { provinceID: id},
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = "<option value = '' style = 'display : none;'>--Chọn quận/huyện--</option>";
                    $.each(data, function (i, item) {
                        html += "<option value = '" + item.ID + "'>" + item.Name + "</option>";
                    });
                    $("#ddlDistrict").html(html);
                }
            }
        })
    },
    loadSubDistrict: function (idPro, idDis) {
        $.ajax({
            url: '/User/LoadSubDistrict',
            type: "POST",
            data: {infor : JSON.stringify({ "ProvinceID": idPro, "DistrictID": idDis, "ID" : 0, "Name" : "NULL" }) },
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    var data = res.data;
                    var html = "<option value = '' style = 'display : none;'>--Chọn xã/phường--</option>";
                    $.each(data, function (i, item) {
                        html += "<option value = '" + item.ID + "'>" + item.Name + "</option>";
                    });
                    $("#ddlSubDistrict").html(html);
                }
            }
        })
    }
}
user.init();
var idProvince = -1;
var idDistrict = -1;