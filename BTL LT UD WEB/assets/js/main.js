//Các kỹ thuật xử lý kịch bản cho index.html
//Khai báo biến


	var def = "---từ khóa---";
	var emp = "";
	
	
	function setFirstTime (fn) {
		//window.document.frmSearch.txtKeyWord.value = def; (C1)
		//var fn = window.document.frmSearch; (C2)
		
		fn.txtKeyWord.value = def;
		
	}
	
	function setKeyWord (fn, isClick) {
		//Lấy giá trị trong hộp từ khóa
		
		var value = fn.txtKeyWord.value;
		
		if(isClick) {
			if(value.trim() == def)
				fn.txtKeyWord.value= emp;
		
		}
		else{
		
		if(value.trim() == emp) {
			fn.txtKeyWord.value = def;
		}
			}
	}
	
	
	function checkValidKeyword (fn) {
		var value = fn.txtKeyWord.value;
		
		if((value.trim()== def) || (value.trim() == emp)) {
			var message = "Hãy nhập vào từ khóa tìm kiếm!" ;
			window.alert(message);
			fn.txtKeyWord.focus();
			fn.txtKeyWord.select();
			
		}
	}