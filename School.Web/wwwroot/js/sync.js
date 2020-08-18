function valueChange() {
        var courses = document.getElementById('courses').ej2_instances[0];
        var value = document.getElementById('value');
        var text = document.getElementById('text');
        value.innerHTML = courses.value === null ? 'null' : courses.value.toString();
        text.innerHTML = courses.text === null ? 'null' : courses.text.toString();
    }