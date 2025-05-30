﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo.Api.Models;

namespace ToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController(SeedTodoData seedTodoData) : ControllerBase
    {
        public SeedTodoData SeedTodoData { get; } = seedTodoData;

        [HttpGet("")]
        public IActionResult Get()
            => Ok(SeedTodoData.Get());

        [HttpGet("{completed:bool}/{status?}")]
        public IActionResult Get(bool completed, string status = "not-started-yet")
        {
            if (string.IsNullOrEmpty(status))
                return Ok(SeedTodoData.GetByCompleted(completed));
            
            var todoItems = SeedTodoData.GetByCompletedStatus(completed, status);
            
            if (todoItems == null || todoItems.Count == 0)
                return Ok();
            
            return Ok(todoItems);
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var todoItem = SeedTodoData.Get(id);

            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] TodoItem todoItem)
        {
            if (todoItem == null)
                return BadRequest("Todo item cannot be null");
            var isAdded = SeedTodoData.Add(todoItem);

            if (!isAdded)
                return BadRequest("Failed to add todo item");

            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }

        [HttpPut("{id:long}")]
        public IActionResult Put(long id, [FromBody] TodoItem todoItem)
        {
            if (todoItem == null)
                return BadRequest("Todo item cannot be null");

            var isUpdated = SeedTodoData.Update(id, todoItem);

            if (!isUpdated)
                return NotFound();

            var todo = SeedTodoData.Get(id);

            return Ok(todo);
        }

        [HttpPut("update-status/{id:long}")]
        public IActionResult Put(long id, [FromBody] UpdateStatus updateStatus)
        {
            var isUpdated = SeedTodoData.Update(id, updateStatus.status);

            if (!isUpdated)
                return NotFound();
            var todo = SeedTodoData.Get(id);
            return Ok(todo);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var isDeleted = SeedTodoData.Delete(id);

            if (!isDeleted)
                return NotFound();

            return Ok();
        }
    }

    public class UpdateStatus 
    {
        public string status { get; set; }
    }
}
