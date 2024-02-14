

document.addEventListener('DOMContentLoaded', function () {
    var createPetitionForm = document.getElementById('createPetitionForm');

    createPetitionForm.addEventListener('submit', function (e) {
        e.preventDefault();

        var formData = new FormData(createPetitionForm);

        fetch('/Petition/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Title: formData.get('Title'),
                Description: formData.get('Description'),
                ImageUrl: formData.get('Image')
            })
        })
            .then(response => response.json())
            .then(data => {
                console.log('Petition created:', data);
                // Optionally, you can redirect or update the UI after successful creation
            })
            .catch(error => console.error('Error creating petition:', error));
    });
});


var resizableForm = document.getElementById('resizableForm');
var textarea = resizableForm.querySelector('textarea');
var headerHeight = document.querySelector('header').offsetHeight;
var footer = document.querySelector('footer');
var footerHeight = footer.offsetHeight;
var footerTop = footer.offsetTop;

textarea.addEventListener('input', function () {
    resizableForm.classList.add('expanded');

    var totalAvailableHeight = window.innerHeight - (headerHeight + footerHeight);
    var maxScrollHeight = footerTop - resizableForm.offsetTop - 20; // 20 is a buffer for spacing
    resizableForm.scrollTop = Math.min(resizableForm.scrollHeight, totalAvailableHeight, maxScrollHeight);
});









[HttpDelete("{id}")]
public async Task < IActionResult > DeletePetition(int id)
{
    var petition = await _context.Petitions.FindAsync(id);
    if (petition == null) {
        return NotFound();
    }

    _context.Petitions.Remove(petition);
    await _context.SaveChangesAsync();

    return NoContent();
}
