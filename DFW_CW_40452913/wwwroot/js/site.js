

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





document.getElementById('searchInput').addEventListener('input', function () {
    var searchText = this.value.toLowerCase();
    var voteFilter = document.getElementById('voteFilter').value;
    var petitionItems = document.querySelectorAll('.petition-item');

    petitionItems.forEach(function (item) {
        var title = item.querySelector('.PetitionTitle').textContent.toLowerCase();
        var counter = parseInt(item.querySelector('.Counter').textContent);

        var matchesSearch = title.includes(searchText);
        var matchesVoteFilter = voteFilter === 'all' || (voteFilter === '0' && counter === 0) || (voteFilter === '1' && counter === 1);

        if (matchesSearch && matchesVoteFilter) {
            item.style.display = 'block';
        } else {
            item.style.display = 'none';
        }
    });
});

document.getElementById('voteFilter').addEventListener('change', function () {
    var searchText = document.getElementById('searchInput').value.toLowerCase();
    var voteFilter = this.value;
    var petitionItems = document.querySelectorAll('.petition-item');

    petitionItems.forEach(function (item) {
        var title = item.querySelector('.PetitionTitle').textContent.toLowerCase();
        var counter = parseInt(item.querySelector('.Counter').textContent);

        var matchesSearch = title.includes(searchText);
        var matchesVoteFilter = voteFilter === 'all' || (voteFilter === '0' && counter === 0) || (voteFilter === '1' && counter === 1);

        if (matchesSearch && matchesVoteFilter) {
            item.style.display = 'block';
        } else {
            item.style.display = 'none';
        }
    });
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




//User Blocked

$(function () {
    $('form[action="/Admin/BlockUser"]').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var userId = form.find('input[name="userId"]').val();

        $.post(form.attr('action'), { userId: userId })
            .done(function (response) {
                if (response.success) {
                    toastr.success('User has been blocked successfully.');
                } else {
                    toastr.error('An error occurred while blocking the user.');
                }
            })
            .fail(function () {
                toastr.error('An error occurred while blocking the user.');
            });
    });
});






//Admin moving user table
// Get the table wrapper element
const tableWrapper = document.querySelector('.table-wrapper');

// Track mouse movements on the table wrapper
tableWrapper.addEventListener('mousedown', (event) => {
    // Get the initial mouse position
    const initialX = event.clientX;
    const initialY = event.clientY;

    // Get the initial position of the table
    const initialTableX = tableWrapper.offsetLeft;
    const initialTableY = tableWrapper.offsetTop;

    // Calculate the offset between the mouse position and the table position
    const offsetX = initialX - initialTableX;
    const offsetY = initialY - initialTableY;

    // Track mouse movements while dragging
    const onMouseMove = (event) => {
        // Calculate the new position of the table
        const newX = event.clientX - offsetX;
        const newY = event.clientY - offsetY;

        // Set the new position of the table
        tableWrapper.style.left = `${newX}px`;
        tableWrapper.style.top = `${newY}px`;
    };

    // Stop tracking mouse movements when the mouse is released
    const onMouseUp = () => {
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
    };

    // Add event listeners to track mouse movements and stop tracking when the mouse is released
    document.addEventListener('mousemove', onMouseMove);
    document.addEventListener('mouseup', onMouseUp);
});
