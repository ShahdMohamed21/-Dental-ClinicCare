document.addEventListener('DOMContentLoaded', function () {
    // 1. تحديد العناصر (الزر والحالات المخفية)
    const btn = document.getElementById('toggle-btn');
    // استخدمنا querySelectorAll لتحديد كل الحالات التي تحتوي على الكلاس extra-case
    const extraCases = document.querySelectorAll('.extra-case');

    // 2. التحقق من وجود الزر قبل محاولة إضافة الـ Listener
    if (btn) {
        btn.addEventListener("click", function () {
            if (btn.innerText === "عرض المزيد") {
                // إظهار الحالات المخفية
                extraCases.forEach(card => card.classList.remove("d-none"));
                btn.innerText = "عرض أقل";
            } else {
                // إخفاء الحالات وعمل تمرير ناعم للأعلى
                extraCases.forEach(card => card.classList.add("d-none"));
                btn.innerText = "عرض المزيد";

                // التمرير إلى بداية القسم
                document.getElementById("cases-container").scrollIntoView({ behavior: "smooth" });
            }
        });
    } else {
        console.error("Button with ID 'toggle-btn' not found.");
    }
});