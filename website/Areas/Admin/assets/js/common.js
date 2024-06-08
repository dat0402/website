
function load() {
    $('#responsive').show();
}
function loadfrmEditSussess() {
    $('#responsive').show();
}
function closedialog() {
    $("#responsive").hide();
}
function onAddSuccess(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            $("#responsive").hide();
            setTimeout(function () {
                location.reload();
            }, 3000)
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function don(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            $("#responsive").hide();
            setTimeout(function () {
                location.reload();
            }, 3000)
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function onEditSuccess(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(300);
            }, 1000)
            $("#responsive").hide();
            setTimeout(function () {
                location.reload();
            }, 3000)
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(300);
            }, 1000)
        }
    }
}
function btndelete(id, control) {

    $('.message-delete').show();    //Event cancel dialog
    $('.huy').click(function () {
        $('.message-delete').hide();
    })
    $('.xoa').on('click', function () {
        debugger;
        $.ajax({
            url: `/Admin/${control}/Delete?id=` + id,
        }).done(function (res) {
            debugger;
            $('.message-delete').hide();
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "#e5e5e5");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 1000)
            setTimeout(function () {
                location.reload();
            }, 3000)
            
        }).fail(function (res) {
            $('.message-delete').hide();
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "#e5e5e5");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        })
    })
}
function btnGiaoHang(id, control) {

    $('.message-giao').show();    //Event cancel dialog
    $('.huy').click(function () {
        $('.message-delete').hide();
    })
    $('.xoa').on('click', function () {
        debugger;
        $.ajax({
            url: `/Admin/${control}/GiaoHang?id=` + id,
        }).done(function (res) {
            debugger;
            $('.message-giao').hide();
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "#e5e5e5");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            location.reload();
        }).fail(function (res) {
            $('.message-giao').hide();
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "#e5e5e5");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        })
    })
}
function login(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 6000)
            if (res.phanquyen == 1) {
                location.href = "/Admin/Homes/Index";
            }
            else {
                location.href = "/Home/Index";
            }
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function doimk(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            location.href = "/Home/Index";
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function register(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            $("#register").fadeOut(600);
            $("body").removeClass("no-scroll");

        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function dangcv(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            setTimeout(function () {
                window.location.href = "/Home/DanhSachCV?id=" + res.id;
            }, 3000)

        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function nopcv(res) {
    debugger;
    if (res.status) {
        if (res.message != undefined) {
            debugger;
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            window.location.href = "/Home/DanhSachCV";
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function thongtin(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            location.href = "/Home/Index";
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function xoagh(id, idbnt, idmau) {
    $.ajax({
        url: '/Cart/Xoa?Id=' + id + '&&IdBNT=' + idbnt + '&&IdMau=' + idmau,
    }).done(function (res) {
        debugger;
        $('.message-delete').hide();
        $('p#notify').text(res.message);
        $('.message-box').css("background-color", "#e5e5e5");
        $('.message-model').show();
        setTimeout(function () {
            $('.message-model').fadeOut(600);
        }, 3000)
        location.reload();
    }).fail(function (res) {
        $('.message-delete').hide();
        $('p#notify').text(res.message);
        $('.message-box').css("background-color", "#e5e5e5");
        $('.message-model').show();
        setTimeout(function () {
            $('.message-model').fadeOut(600);
        }, 3000)
    })
}
function dathang(res) {
    if (res.status) {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
            location.href = "/Home/Index";
        }
    } else {
        if (res.message != undefined) {
            $('p#notify').text(res.message);
            $('.message-box').css("background-color", "white");
            $('.message-model').show();
            setTimeout(function () {
                $('.message-model').fadeOut(600);
            }, 3000)
        }
    }
}
function ungtuyen(id, idtk) {
    if (idtk == 0) {
        $('p#notify').text("Bạn chưa đăng nhập.");
        $('.message-box').css("background-color", "white");
        $('.message-model').show();
        setTimeout(function () {
            $('.message-model').fadeOut(600);
        }, 3000)
    }
    else {
        window.location.href = "/Home/UngTuyen?id=" + id;
    }
}
function loc() {
    var loc = $('#loc').val();
    window.location.href = "/Admin/DonHang?tinhtrang=" + loc;
}