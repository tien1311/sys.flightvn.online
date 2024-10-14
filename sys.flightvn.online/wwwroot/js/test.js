$(function(){
	var chieungang = $(window).width();
	$('.header').css({'width':chieungang})
	$(window).resize(function(){
		var chieungang = $(window).width();
		$('.header').css({'width':chieungang})
	})
})

$(function(){
	var chieungang = $(window).width();
	$('.contain-video').css({'width':chieungang})
	$(window).resize(function(){
		var chieungang = $(window).width();
		$('.contain-video').css({'width':chieungang})
	})
})

$(function () {
    var chieungang = $(window).width();
    $('p.title').css({ 'width': chieungang })
    $(window).resize(function () {
        var chieungang = $(window).width();
        $('p.title').css({ 'width': chieungang })
    })
})