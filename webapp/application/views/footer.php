<?php
defined('BASEPATH') OR exit('No direct script access allowed');
?>


      <div id="app"></div>

      <br style="clear:both;" />
      <div class="row mt-50 mb-50">
        <div class="col-12 footer">
          <h1>Playfirst &copy; 2017</h1>
          <p>Please report any errors to <a href="mailto:support@playfirst.co.za">support@playfirst.co.za</a>.</p>
        </div>
      </div>

    </div>
  <div class="col-1"></div>
</div>


<script src="<?php echo base_url();?>dist/bundle.js"></script>
<script>
$(window).ready(function() {
  $('#loading-screen').fadeOut();
});
</script>
</body>
</html>
